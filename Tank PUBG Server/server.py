import socket
import struct
import packer
import time
import gl
from net_manager import net_manager
from msg_manager import msg_manager


class Server:

    def __init__(self):

        pass

    def run(self):
        # 连接至physics
        writer = packer.Packer()
        writer.write_int_32(gl.cmd['SP_CONTROL_CONNECT'])
        net_manager.add_msg(writer.get_bytes(), gl.physics_addr)

        while True:
            net_manager.update(msg_manager)

    def get_available_client_id(self):
        res = self.curr_available_client_id
        self.curr_available_client_id += 1
        return res


server = Server()
server.run()
