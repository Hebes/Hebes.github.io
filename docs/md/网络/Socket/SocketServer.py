import socket
host='127.0.0.1'
port=996
print('服务器IP:'+host+'\n')
print('服务器端口号:'+str(port)+'\n')
s=socket.socket(socket.AF_INET,socket.SOCK_STREAM)#创建套接字
s.bind((host,port))#绑定ip和port
s.listen(10)#设置最大监听量
sock,addr=s.accept()
print('连接已经建立')
info=sock.recv(1024).decode()
while True:
    if info:
        print('接收到的内容：'+info)
    send_data=input('输入发送内容：')
sock.send(send_data.encode())
info=sock.recv(1024).decode()
sock.close()
s.close()

