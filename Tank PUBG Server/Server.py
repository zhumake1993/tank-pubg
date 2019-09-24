import socket
import struct
import Packer

socket_server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
socket_server.bind(("127.0.0.1", 10080))

socket_physx = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
physx_addr = ('127.0.0.1', 10081)

client_addr = None


def recv_from_client():

    data, addr = socket_server.recvfrom(1024)
    packer = Packer.Packer(data)
    val1 = packer.read_int_32()
    val2 = packer.read_int_32()
    print('from', addr, data, (val1, val2))

    data, addr = socket_server.recvfrom(1024)
    packer = Packer.Packer(data)
    val1 = packer.read_int_32()
    val2 = packer.read_float()
    val3 = packer.read_float()
    print('from', addr, data, (val1, val2, val3))

    global client_addr
    client_addr=addr

    return val2, val3


def send_to_physx(val2, val3):
    writer = Packer.Packer()
    writer.write_int_32(0)
    writer.write_int_32(1)
    data = writer.get_bytes()
    print('to', physx_addr, data, (0, 1))
    socket_physx.sendto(data, physx_addr)

    writer = Packer.Packer()
    writer.write_int_32(1)
    writer.write_float(val2)
    writer.write_float(val3)
    data = writer.get_bytes()
    print('to', physx_addr, data, (0, val2, val3))
    socket_physx.sendto(data, physx_addr)


def recv_from_physx():
    data, addr = socket_physx.recvfrom(1024)
    packer = Packer.Packer(data)
    val1 = packer.read_int_32()
    val2 = packer.read_int_32()
    print('from', addr, data, (val1, val2))

    data, addr = socket_physx.recvfrom(1024)
    packer = Packer.Packer(data)
    val1 = packer.read_int_32()
    val2 = packer.read_float()
    val3 = packer.read_float()
    print('from', addr, data, (val1, val2, val3))

    return val2, val3


def send_to_client(val2, val3):
    writer = Packer.Packer()
    writer.write_int_32(0)
    writer.write_int_32(1)
    data = writer.get_bytes()
    print('to', client_addr, data, (0, 1))
    socket_server.sendto(data, client_addr)

    writer = Packer.Packer()
    writer.write_int_32(1)
    writer.write_float(val2)
    writer.write_float(val3)
    data = writer.get_bytes()
    print('to', client_addr, data, (1, val2, val3))
    socket_server.sendto(data, client_addr)


while True:
    print("")
    v, h = recv_from_client()
    send_to_physx(v, h)
    v, h = recv_from_physx()
    send_to_client(v, h)
