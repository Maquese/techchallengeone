terraform {
  required_version = ">= 1.0"
  required_providers {
    null = {
      source  = "hashicorp/null"
      version = ">= 3.0"
    }
  }
}

resource "null_resource" "apply_k8s_manifests" {
  triggers = {
    manifest_hash = join("|", [
      filemd5(abspath("${path.module}/../kub/volume.yaml")),
      filemd5(abspath("${path.module}/../kub/persistClaim.yaml")),
      filemd5(abspath("${path.module}/../kub/deployment-sql.yaml")),
      filemd5(abspath("${path.module}/../kub/service-sql.yaml")),
      filemd5(abspath("${path.module}/../kub/environment.yaml")),
      filemd5(abspath("${path.module}/../kub/deployment.yaml")),
      filemd5(abspath("${path.module}/../kub/service.yaml")),
      filemd5(abspath("${path.module}/../kub/metrics.yaml")),
      filemd5(abspath("${path.module}/../kub/hpa.yaml")),
    ])
  }

  provisioner "local-exec" {
    command = "kubectl apply -f \"${abspath("${path.module}/../kub/volume.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/persistClaim.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/deployment-sql.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/service-sql.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/environment.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/deployment.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/service.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/metrics.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../kub/hpa.yaml")}\""
  }
}
