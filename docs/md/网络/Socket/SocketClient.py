import socket
s=socket.socket()
host='127.0.0.1'
port=996
s.connect((host,port))#连接服务器
print('已连接')
info=''
while True:
    send_data=input('输入发送内容：')
    s.send(send_data.encode())
    info=s.recv(1024).decode()
    print('接收的内容：'+info)
s.close()

