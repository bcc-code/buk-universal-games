resource "google_compute_global_address" "default" {
  provider = google-beta
  name     = "load-balancer-static-ip-${var.environment-name}"
}

resource "google_compute_global_forwarding_rule" "default" {
  name                  = "load-balancer-frontend-${var.environment-name}"
  provider              = google-beta
  ip_protocol           = "TCP"
  load_balancing_scheme = "EXTERNAL"
  port_range            = "443"
  target                = google_compute_target_https_proxy.default.id
  ip_address            = google_compute_global_address.default.id
}

resource "google_compute_managed_ssl_certificate" "default" {
  name = "load-balancer-ssl-cert-${var.environment-name}"

  managed {
    domains = [var.domain-name]
  }
}

resource "google_compute_target_https_proxy" "default" {
  name             = "load-balancer-target-https-proxy-${var.environment-name}"
  url_map          = google_compute_url_map.default.id
  ssl_certificates = [google_compute_managed_ssl_certificate.default.id]
}

resource "google_compute_url_map" "default" {
  name     = "load-balancer-url-map-${var.environment-name}"
  provider = google-beta
  default_url_redirect {
    host_redirect          = "bcc.no"
    strip_query            = true
    redirect_response_code = "MOVED_PERMANENTLY_DEFAULT"
  }
  host_rule {
    hosts        = [var.domain-name]
    path_matcher = "default-matcher"
  }
  path_matcher {
    name = "default-matcher"
    path_rule {
      paths   = ["/api/*", "/api"]
      service = module.buk-universal-games-api.backend-service.id
    }
    path_rule {
      paths   = ["/*", "/"]
      service = module.buk-universal-games-site.backend-service.id
    }
    default_url_redirect {
      host_redirect          = "bcc.no"
      strip_query            = true
      redirect_response_code = "MOVED_PERMANENTLY_DEFAULT"
    }
  }
}
