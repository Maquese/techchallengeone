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
      filemd5(abspath("${path.module}/../k8s/volume.yaml")),
      filemd5(abspath("${path.module}/../k8s/persistClaim.yaml")),
      filemd5(abspath("${path.module}/../k8s/deployment-sql.yaml")),
      filemd5(abspath("${path.module}/../k8s/service-sql.yaml")),
      filemd5(abspath("${path.module}/../k8s/secret.yaml")),
      filemd5(abspath("${path.module}/../k8s/environment.yaml")),
      filemd5(abspath("${path.module}/../k8s/deployment.yaml")),
      filemd5(abspath("${path.module}/../k8s/service.yaml")),
      filemd5(abspath("${path.module}/../k8s/metrics.yaml")),
      filemd5(abspath("${path.module}/../k8s/components.yaml")),
      filemd5(abspath("${path.module}/../k8s/hpa.yaml")),
    ])
  }

  provisioner "local-exec" {
    command = "kubectl apply -f \"${abspath("${path.module}/../k8s/volume.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/persistClaim.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/deployment-sql.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/service-sql.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/secret.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/environment.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/deployment.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/service.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/metrics.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/components.yaml")}\" && kubectl apply -f \"${abspath("${path.module}/../k8s/hpa.yaml")}\""
  }
}
