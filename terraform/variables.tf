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

variable "rest_service_ip" {
  description = "The IP address of the REST service"
}

variable "rest_service_port" {
  description = "The port of the REST service"
}

variable "atlas_chart_embed_dashboard_url" {
  description = "The URL of the embedded Atlas dashboard"
}

variable "atlas_chart_id_event" {
  description = "The ID of the event chart"
}

variable "atlas_chart_id_player" {
  description = "The ID of the player chart"
}

variable "atlas_chart_id_home" {
  description = "The ID of the home chart"
}

variable "atlas_chart_id_similar" {
  description = "The ID of the similar chart"
}

variable "game_client_port" {
  description = "The port of the game client"
  default     = 80
}

variable "branch_name" {
  description = "The name of the branch"
}

