resource "aws_acm_certificate" "leafsteroids" {
  certificate_authority_arn = null
  domain_name               = "leafsteroids.net"
  key_algorithm             = "RSA_2048"
  subject_alternative_names = [
    "api.leafsteroids.net",
    "api.staging.leafsteroids.net",
    "leafsteroids.net",
    "staging.leafsteroids.net",
  ]
  tags              = local.tags
  validation_method = "DNS"

  options {
    certificate_transparency_logging_preference = "ENABLED"
  }
}
