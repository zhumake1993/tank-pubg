import gl
from net_manager import net_manager
import packer


class MsgManager:

    def __init__(self):
        # physics消息处理跳表
        self.handle_physics_msg = {
            gl.cmd['PS_CONTROL_CONNECT_SUCCESS']: self.handle_ps_control_connect_success,
            gl.cmd['PS_SYN_INSTANTIATE']: self.handle_ps_syn_instantiate,
            gl.cmd['PS_SYN_TRANSFORM']: self.handle_ps_syn_transform,
            gl.cmd['PS_SYN_DESTROY']: self.handle_ps_syn_destroy
        }

        # client消息处理跳表
        self.handle_client_msg = {
            gl.cmd['CS_CONTROL_CONNECT']: self.handle_cs_control_connect,
            gl.cmd['CS_GAME_START']: self.handle_cs_game_start,
            gl.cmd['CS_INPUT']: self.handle_cs_input
        }

    def handle(self, data, addr):
        reader = packer.Packer(data)
        cmd = reader.read_int_32()

        if addr == gl.physics_addr:
            self.handle_physics_msg[cmd](reader)
        else:
            self.handle_client_msg[cmd](reader, addr)

    # 处理来自physics的消息

    def handle_ps_control_connect_success(self, reader):
        print('Connect Physics Success!')

    def handle_ps_syn_instantiate(self, reader):
        entity_id = reader.read_int_32()
        client_id = reader.read_int_32()
        name = reader.read_string()
        px = reader.read_float()
        py = reader.read_float()
        pz = reader.read_float()
        rx = reader.read_float()
        ry = reader.read_float()
        rz = reader.read_float()
        rw = reader.read_float()

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_SYN_INSTANTIATE'])
        writer.write_int_32(entity_id)
        writer.write_int_32(client_id)
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

    def handle_ps_syn_destroy(self, reader):
        entity_id = reader.read_int_32()

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_SYN_DESTROY'])
        writer.write_int_32(entity_id)
        data = writer.get_bytes()
        for client_addr in self.client_addrs:
            self.socket_server.sendto(data, client_addr)

    # 处理来自客户端的消息

    def handle_cs_control_connect(self, reader, addr):
        gl.client_addrs.append(addr)
        gl.client_id[addr] = gl.curr_available_client_id
        gl.curr_available_client_id += 1

        writer = packer.Packer()
        writer.write_int_32(gl.cmd['SC_CONTROL_CONNECT_SUCCESS'])
        writer.write_int_32(gl.client_id[addr])
        data = writer.get_bytes()
        self.socket_server.sendto(data, addr)

    def handle_cs_game_start(self, reader, addr):
        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SP_GAME_START'])
        data = writer.get_bytes()
        self.socket_server.sendto(data, self.physics_addr)

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SC_GAME_START'])
        data = writer.get_bytes()
        for client_addr in self.client_addrs:
            self.socket_server.sendto(data, client_addr)

    def handle_cs_input(self, reader, addr):
        v = reader.read_float()
        h = reader.read_float()
        a = reader.read_float()
        j = reader.read_int_32()
        client_id = self.client_id[addr]

        writer = Packer.Packer()
        writer.write_int_32(self.cmd['SP_INPUT'])
        writer.write_int_32(client_id)
        writer.write_float(v)
        writer.write_float(h)
        writer.write_float(a)
        writer.write_int_32(j)
        data = writer.get_bytes()
        self.socket_server.sendto(data, self.physics_addr)


msg_manager = MsgManager()
