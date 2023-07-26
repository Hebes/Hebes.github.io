/*************************************************
	作者: Plane
	邮箱: 1785275942@qq.com	
	功能: 表格数据读取与解压

    //=================*=================\\
           教学官网：www.qiqiker.com
           官方微信服务号: qiqikertuts
           Plane老师微信: PlaneZhong
               ~~获取更多教学资讯~~
    \\=================*=================//
*************************************************/

using System.IO;

public class ExcelTool {
    //Load File in StreamingAssets Folder
    public static byte[] LoadData(string path) {
        byte[] data = null;

        using(Stream file = File.OpenRead(path)) {
            data = new byte[(int)file.Length];
            file.Read(data, 0, data.Length);
            file.Close();
            file.Dispose();
        }
        return data;
    }

    public static byte[] Decompress(byte[] input) {
        MemoryStream ms = new MemoryStream(input);
        var zos = new zlib.ZInputStream(ms);
        MemoryStream output = new MemoryStream();
        byte[] temp = new byte[4096];
        int len;
        while((len = zos.read(temp, 0, temp.Length)) > 0) {
            output.Write(temp, 0, len);
        };
        zos.Close();
        return output.ToArray();
    }
}
