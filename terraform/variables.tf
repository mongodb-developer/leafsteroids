variable "environment" {
  description = "The environment to deploy the resources to"
  default     = "prod"
}

variable "mongodb_database" {
  description = "The name of the MongoDB database"
}

variable "vpc_id" {
  description = "The ID of the VPC"
}

variable "subnet_ids" {
  description = "The IDs of the subnets"
}

variable "security_group_id" {
  description = "The ID of the security group"
}

variable "mongodb_uri" {
  description = "The URI of the MongoDB database"
}

variable "account_id" {
  description = "The AWS account ID"
}

variable "connection_arn" {
  description = "The ARN of the CodePipeline connection to the GitHub repo"
}
