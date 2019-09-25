import socket
import struct
import Packer
import threading


class Server:

    def __init__(self):
        self.path = 'F:\\Tank PUBG\\'
        self.max_size = 512

        self.socket_server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.socket_server.bind(("127.0.0.1", 10080))

        # self.socket_physics = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.physics_addr = ('127.0.0.1', 10081)

        self.cmd = {}
        self.load_cmd()

        self.client_addrs = []

        self.handle_physics_msg = {
            self.cmd['PS_CONTROL_CONNECT_SUCCESS']: self.handle_ps_control_connect_success,
            self.cmd['PS_INSTANTIATE']: self.handle_ps_instantiate,
            self.cmd['PS_SYN_TRANSFORM']: self.handle_ps_syn_transform
        }

        self.handle_client_msg = {
            self.cmd['CS_CONTROL_CONNECT']: self.handle_cs_control_connect,
            self.cmd['CS_GAME_START']: self.handle_cs_game_start
        }

    def load_cmd(self):
        index = 0
        with open(self.path + 'cmd.txt', 'r', encoding='utf-8') as myfile:
            for line in myfile:
                line = line.strip()
                if len(line) == 0:
                    continue
                sub = line.split()
                if sub[0] == 'Command':
                    continue
                self.cmd[sub[0]] = index
                index += 1

    def run(self):
        # 启动监听
        thread_recv = threading.Thread(target=self.recv_thread, args=())
        thread_recv.setDaemon(True)
        thread_recv.start()

        # 连接至physics
        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SP_CONTROL_CONNECT'])
        data = writer.get_bytes()
        self.socket_server.sendto(data, self.physics_addr)

        while True:
            cmd = input('Please enter command:\n')
            print(cmd)

    def recv_thread(self):
        while True:
            data, addr = self.socket_server.recvfrom(self.max_size)

            reader = Packer.Packer(data)
            cmd = reader.read_int_32()

            if addr == self.physics_addr:
                self.handle_physics_msg[cmd](reader)
            else:
                self.handle_client_msg[cmd](reader, addr)

    # 处理来自physics的消息

    def handle_ps_control_connect_success(self, reader):
        print('Connect Physics Success!')

    def handle_ps_instantiate(self, reader):
        entity_id = reader.read_int_32()
        name = reader.read_string()
        px = reader.read_float()
        py = reader.read_float()
        pz = reader.read_float()
        rx = reader.read_float()
        ry = reader.read_float()
        rz = reader.read_float()
        rw = reader.read_float()

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_INSTANTIATE'])
        writer.write_int_32(entity_id)
        writer.write_string(name)
        writer.write_float(px)
        writer.write_float(py)
        writer.write_float(pz)
        writer.write_float(rx)
        writer.write_float(ry)
        writer.write_float(rz)
        writer.write_float(rw)

        data = writer.get_bytes()
        for client_addr in self.client_addrs:
            self.socket_server.sendto(data, client_addr)

    def handle_ps_syn_transform(self, reader):
        entity_id = reader.read_int_32()
        px = reader.read_float()
        py = reader.read_float()
        pz = reader.read_float()
        rx = reader.read_float()
        ry = reader.read_float()
        rz = reader.read_float()
        rw = reader.read_float()

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_SYN_TRANSFORM'])
        writer.write_int_32(entity_id)
        writer.write_float(px)
        writer.write_float(py)
        writer.write_float(pz)
        writer.write_float(rx)
        writer.write_float(ry)
        writer.write_float(rz)
        writer.write_float(rw)

        data = writer.get_bytes()
        for client_addr in self.client_addrs:
            self.socket_server.sendto(data, client_addr)

    # 处理来自客户端的消息

    def handle_cs_control_connect(self, reader, addr):
        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_CONTROL_CONNECT_SUCCESS'])
        data = writer.get_bytes()
        self.socket_server.sendto(data, addr)
        self.client_addrs.append(addr)

    def handle_cs_game_start(self, reader, addr):
        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SP_GAME_START'])
        data = writer.get_bytes()
        self.socket_server.sendto(data, self.physics_addr)


server = Server()
server.run()
