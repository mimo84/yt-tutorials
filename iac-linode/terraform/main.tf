provider "linode" {
  token = var.linode_access_token
}

provider "tls" {}

resource "tls_private_key" "ed25519-linode" {
  algorithm = "ED25519"
}

data "tls_public_key" "private_key_openssh-linode" {
  private_key_openssh = file(var.linode_root_ssh_privatekey)
}

resource "linode_instance" "web_server" {
  image           = "linode/ubuntu24.04"
  label           = "linode-terraform"
  region          = "ap-southeast"
  type            = "g6-nanode-1"
  tags            = ["hello-terraform"]
  root_pass       = var.linode_root_password
  authorized_keys = [chomp(data.tls_public_key.private_key_openssh-linode.public_key_openssh)]

  connection {
    type        = "ssh"
    user        = "root"
    host        = self.public_ip
    private_key = file(var.linode_root_ssh_privatekey)
  }

  provisioner "local-exec" {
    command = "export ANSIBLE_HOST_KEY_CHECKING=False && ansible-playbook -u root -i '${self.ip_address},' --private-key='${var.linode_root_ssh_privatekey}' ../ansible/setup.yml"
  }
}



output "instance_ip" {
  value = linode_instance.web_server.ip_address
}
