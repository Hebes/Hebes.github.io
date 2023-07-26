# -*- coding:utf-8 -*-
'''****************************************************
	文件：trans.py
	作者：Plane
	邮箱: 1785275942@qq.com
    官网：www.qiqiker.com
	功能：转化xls表格数据为json、lua、二进制文件
*****************************************************'''

import os
import sys
import json
import xlrd
import zlib
import google
import struct

from PELog import LogInfo, LogMsg, LogError


def calc_relative_path(dir, filename):
    return os.path.join(os.path.dirname(os.path.abspath(__file__)), dir, filename)


cfg_file = open(calc_relative_path("config", "config.json"), encoding='utf-8')
cfg_json = json.load(cfg_file)
exec("import " + cfg_json["proto"] + "_pb2")
exec("proto = " + cfg_json["proto"] + "_pb2")


def generate_cs():
    for path in cfg_json["csPath"]:
        if os.path.exists(path) == False:
            os.makedirs(path)
        os.system(".\soft\protogen.exe --csharp_out=%s %s.proto" % (path, cfg_json["proto"]))


def loop_work():
    LogInfo("start loop work...")
    xls_cfg = cfg_json["xls"]
    for item_cfg in xls_cfg:
        LogInfo("Generate Structure: " + item_cfg["structure"])
        convert_data(item_cfg)


def convert_data(convert_item):
    tClass = getattr(proto, convert_item["structure"])
    tData = tClass()
    sData = tData.lst
    for xls_file in convert_item["files"]:
        LogInfo("Process xls ###===>>> " + xls_file)
        book = xlrd.open_workbook(calc_relative_path("xls", xls_file), formatting_info=True)
        sheets = book.sheets()
        for index, sheet in enumerate(sheets):
            if sheet.name in cfg_json["ignore"]:
                continue
            if sheet.nrows <= cfg_json['titleRow']:  # col:竖列、row:横行
                continue
            LogMsg("Current Trans SheetName: " + sheet.name)
            firstRows = sheet.row(cfg_json['titleRow'])
            firstRow = []
            for col in firstRows:
                firstRow.append(str(col.value))

            desc = calc_structure("", firstRow, tClass.lst.DESCRIPTOR.message_type.fields, True)

            for i in range(cfg_json['startRow'], sheet.nrows):
                row = sheet.row(i)
                if str(row[0].value) == '':
                    continue
                eData = sData.add()
                try:
                    set_value(eData, row, desc, convert_item)
                except:
                    LogError("set value error in:" + xls_file + " Item:" + convert_item['structure'] + " Sheet: " + sheet.name + " Line:" + str(i))
                    assert (False)

        if "jsonPath" in cfg_json:
            if "ConvertJson" in convert_item and convert_item["ConvertJson"]:
                for path in cfg_json["jsonPath"]:
                    if os.path.exists(path) == False:
                        os.makedirs(path)
                    write_json_file(tData, path + convert_item["structure"] + ".json")
        if "bytesPath" in cfg_json:
            if "ConvertBytes" in convert_item and convert_item["ConvertBytes"]:
                for path in cfg_json["bytesPath"]:
                    if os.path.exists(path) == False:
                        os.makedirs(path)
                    write_bytes_file(tData, path + convert_item["structure"] + ".bytes")
        if "luaPath" in cfg_json:
            if "ConvertLua" in convert_item and convert_item["ConvertLua"]:
                for path in cfg_json["luaPath"]:
                    if os.path.exists(path) == False:
                        os.makedirs(path)
                    write_lua_file(tData, path + convert_item["structure"] + ".txt")


def calc_structure(pre, firstRow, fields, isCheck):
    desc = {}
    exist = False
    for field in fields:
        if field.label == field.LABEL_REPEATED:
            desc[field.name] = []
            i = 0
            while True:
                path = pre + field.name + str(i)
                if field.type == field.TYPE_MESSAGE:
                    subDesc = calc_structure(path, firstRow, field.message_type.fields, False)
                    if subDesc:
                        desc[field.name].append(subDesc)
                    else:
                        break
                else:
                    path = pre + field.name + str(i)
                    if path not in firstRow:
                        break
                    desc[field.name].append({'path': path, 'col': firstRow.index(path), 'type': field.type})
                exist = True
                i = i + 1
        else:
            path = pre + field.name
            if field.type == field.TYPE_MESSAGE:
                subDesc = calc_structure(path, firstRow, field.message_type.fields, True)
                if subDesc:
                    desc[field.name] = subDesc
                    exist = True
                else:
                    assert (False)
            else:
                if path in firstRow:
                    desc[field.name] = {'path': path, 'col': firstRow.index(path), 'type': field.type}
                    exist = True
                else:
                    if isCheck and field.label == field.LABEL_REQUIRED:
                        LogError("Missing Required Field: " + path)
                        assert (False)

    if exist:
        return desc
    else:
        return None


def set_value(eData, row, desc, tconfig):
    has = False
    for name, entry in desc.items():
        subData = getattr(eData, name)
        if isinstance(entry, list):
            for sentry in entry:
                if 'col' in sentry:
                    value, subHas = get_value(row, sentry)
                    if subHas or ("keepArray" in tconfig and tconfig["keepArray"]):
                        subData.append(value)
                        has = True
                    else:
                        continue
                else:
                    aData = subData.add()
                    subHas = set_value(aData, row, sentry, tconfig)
                    if subHas or ("keepArray" in tconfig and tconfig["keepArray"]):
                        has = True
                    else:
                        subData.remove(aData)
        else:
            if 'col' in entry:
                value, subHas = get_value(row, entry)
                setattr(eData, name, value)
                if subHas:
                    has = True
            else:
                subHas = set_value(subData, row, entry, tconfig)
                if subHas:
                    has = True
    return has


def get_value(row, entry):
    has = True
    value = row[entry['col']].value
    if value == '':
        has = False

    ctype = entry['type']
    if ctype == google.protobuf.descriptor.FieldDescriptor.TYPE_DOUBLE or ctype == google.protobuf.descriptor.FieldDescriptor.TYPE_FLOAT:
        return (trans_float(value), has)
    elif ctype == google.protobuf.descriptor.FieldDescriptor.TYPE_STRING:
        if isinstance(value, (int, float)):
            value = str(int(value))
        return (value, has)
    else:
        return (trans_int(value), has)


def write_json_file(tData, path):
    tStruct = pb2struct(tData)
    encodedjson = json.dumps(tStruct, sort_keys=True, indent=2, ensure_ascii=False).encode("utf-8")
    f = open(path, "wb+")
    f.write(encodedjson)
    f.close()


def write_lua_file(tData, path):
    tStruct = pb2struct(tData)
    f = open(path, "wb+")
    write_utf_8(f, "return ")
    write_lua_struct(tStruct, f, "", "\n")
    f.close()


def write_lua_struct(tStruct, f, pre, tail):
    if isinstance(tStruct, (int, float)):
        write_utf_8(f, str(tStruct))
    elif isinstance(tStruct, str):
        write_utf_8(f, '"')
        write_utf_8(f, tStruct)
        write_utf_8(f, '"')
    elif isinstance(tStruct, list):
        write_utf_8(f, "{\n")
        for v in tStruct:
            write_utf_8(f, pre + "\t")
            write_lua_struct(v, f, pre + "\t", ",\n")
        write_utf_8(f, pre + "}")
    else:
        write_utf_8(f, "{\n")
        for k in tStruct:
            v = tStruct[k]
            if isinstance(k, (int, float)):
                write_utf_8(f, pre + "\t[" + str(k) + "] = ")
            else:
                write_utf_8(f, pre + "\t" + k + " = ")
                write_lua_struct(v, f, pre + "\t", ",\n")
        write_utf_8(f, pre + "}")
    write_utf_8(f, tail)


def write_bytes_file(tData, path):
    pkg = tData.SerializeToString()
    zpkg = zlib.compress(pkg)
    f = open(path, "wb+")
    f.write(zpkg)
    f.close()


def pb2struct(tData):
    tStruct = {}
    fields = tData.ListFields()
    for type in tData.DESCRIPTOR.fields:
        if type.has_default_value:
            tStruct[type.name] = type.default_value
        if type.label == type.LABEL_REPEATED or type.type == type.TYPE_MESSAGE:
            tStruct[type.name] = []
    for (type, sData) in fields:
        if type.label == type.LABEL_REPEATED:
            tStruct[type.name] = []
            for entry in sData:
                if type.type == type.TYPE_MESSAGE:
                    stStruct = pb2struct(entry)
                    tStruct[type.name].append(stStruct)
                else:
                    tStruct[type.name].append(entry)
        else:
            if type.type == type.TYPE_MESSAGE:
                stStruct = pb2struct(sData)
                tStruct[type.name] = stStruct
            else:
                tStruct[type.name] = sData
    return tStruct


def write_utf_8(f, context):
    f.write(context.encode('utf-8'))


def trans_float(value):
    value = str(value)
    if value == '':
        return 0
    else:
        return float(value)


def trans_int(value):
    return int(trans_float(value))


loop_work()
generate_cs()
# os.system("pause")