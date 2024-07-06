resource "linode_domain" "domain" {
  domain    = var.linode_domain
  soa_email = "hostmaster@${var.linode_domain}"
  type      = "master"
}

resource "linode_domain_record" "a" {
  domain_id   = linode_domain.domain.id
  record_type = "A"
  name        = ""
  target      = linode_instance.web_server.ip_address
}

resource "linode_domain_record" "aaaa" {
  domain_id   = linode_domain.domain.id
  record_type = "AAAA"
  name        = ""
  target      = element(split("/", linode_instance.web_server.ipv6), 0)
}

resource "linode_domain_record" "cname-www" {
  domain_id   = linode_domain.domain.id
  record_type = "CNAME"
  name        = "www"
  target      = var.linode_domain
}

resource "linode_domain_record" "cname-mail" {
  domain_id   = linode_domain.domain.id
  record_type = "MX"
  name        = ""
  target      = "mail.${var.linode_domain}"
  priority    = "10"
}

resource "linode_domain_record" "txt-spf" {
  domain_id   = linode_domain.domain.id
  record_type = "TXT"
  name        = ""
  target      = "v=spf1 mx -all"
}

resource "linode_domain_record" "txt-dmarc" {
  domain_id   = linode_domain.domain.id
  record_type = "TXT"
  name        = "_dmarc"
  target      = "v=DMARC1; p=reject"
}

resource "linode_domain_record" "caa" {
  domain_id   = linode_domain.domain.id
  record_type = "CAA"
  name        = ""
  target      = "letsencrypt.org"
  tag         = "issue"
}
