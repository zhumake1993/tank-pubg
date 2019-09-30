import socket
import threading
import gl


class NetManager:

    def __init__(self):
        # socket相关
        self.socket_server = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
        self.socket_server.bind(gl.server_addr)
        self.physics_addr = gl.physics_addr

        # 读写列表和读写锁
        self.send_list = []
        self.recv_list = []
        self.lock_send = threading.Lock()
        self.lock_recv = threading.Lock()

        # 启动监听
        thread_recv = threading.Thread(target=self.recv_msg_thread, args=())
        thread_recv.setDaemon(True)
        thread_recv.start()

    def add_msg(self, msg, addr):
        self.lock_send.acquire()
        self.send_list.append((msg, addr))
        self.lock_send.release()

    def send_msg(self):
        self.lock_send.acquire()
        for (msg, addr) in self.send_list:
            self.socket_server.sendto(msg, addr)
        self.send_list = []
        self.lock_send.release()

    def recv_msg_thread(self):
        while True:
            data, addr = self.socket_server.recvfrom(gl.max_size)
            self.lock_recv.acquire()
            self.recv_list.append((data, addr))
            self.lock_recv.release()

    def handle_msg(self, handler):
        self.lock_recv.acquire()

        for (data, addr) in self.recv_list:
            handler.handle(data, addr)

        self.recv_list = []
        self.lock_recv.release()

    def update(self, handler):
        self.send_msg()
        self.handle_msg(handler)


net_manager = NetManager()
