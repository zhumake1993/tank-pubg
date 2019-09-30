import struct


class Packer:

    def __init__(self, bytes=b''):
        self.bytes = bytes
        self.seeker = 0

    # 读方法

    def read_int_64(self):
        value = self.bytes[self.seeker:self.seeker + 8]
        self.seeker += 8
        return struct.unpack('>q', value)[0]

    def read_int_32(self):
        value = self.bytes[self.seeker:self.seeker + 4]
        self.seeker += 4
        return struct.unpack('>i', value)[0]

    def read_int_16(self):
        value = self.bytes[self.seeker:self.seeker + 2]
        self.seeker += 2
        return struct.unpack('>h', value)[0]

    def read_byte(self):
        value = self.bytes[self.seeker:self.seeker + 1]
        self.seeker += 1
        return struct.unpack('>b', value)[0]

    def read_float(self):
        value = self.bytes[self.seeker:self.seeker + 4]
        self.seeker += 4
        return struct.unpack('>f', value)[0]

    def read_double(self):
        value = self.bytes[self.seeker:self.seeker + 8]
        self.seeker += 8
        return struct.unpack('>d', value)[0]

    def read_string(self):
        length = self.read_int_32()
        value = self.bytes[self.seeker:self.seeker + length]
        self.seeker += length
        return struct.unpack('>' + str(length) + 's', value)[0]

    # 写方法

    def write_int_64(self, value):
        self.bytes += struct.pack('>q', value)

    def write_int_32(self, value):
        self.bytes += struct.pack('>i', value)

    def write_int_16(self, value):
        self.bytes += struct.pack('>h', value)

    def write_byte(self, value):
        self.bytes += struct.pack('>b', value)

    def write_float(self, value):
        self.bytes += struct.pack('>f', value)

    def write_double(self, value):
        self.bytes += struct.pack('>d', value)

    def write_string(self, value):
        self.bytes += struct.pack('>i', len(value))
        self.bytes += struct.pack('>' + str(len(value)) + 's', value)

    def get_bytes(self):
        return self.bytes

    def get_length(self):
        return len(self.bytes)
