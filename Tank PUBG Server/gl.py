path = 'F:\\Tank PUBG\\'
max_size = 512

server_addr = ('127.0.0.1', 10080)
physics_addr = ('127.0.0.1', 10081)

cmd = {}

# 客户的地址
client_addrs = []

# 客户id
curr_available_client_id = 1
client_id = {}


# 读取指令
def load_cmd():
    index = 0
    with open(path + 'cmd.txt', 'r', encoding='utf-8') as myfile:
        for line in myfile:
            line = line.strip()
            if len(line) == 0:
                continue
            sub = line.split()
            if sub[0] == '//':
                continue
            cmd[sub[0]] = index
            index += 1


load_cmd()
