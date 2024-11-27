locals {
  website_name = "leafsteroids-website-${var.environment}"
}

resource "aws_ecr_repository" "leafsteroids-website" {
  image_tag_mutability = "MUTABLE"
  name                 = "leafsteroids/${local.website_name}"
  tags                 = local.tags

  encryption_configuration {
    encryption_type = "AES256"
    kms_key         = null
  }

  image_scanning_configuration {
    scan_on_push = false
  }
}

resource "aws_ecr_lifecycle_policy" "leafsteroids-website" {
  policy = jsonencode(
    {
      rules = [
        {
          action = {
            type = "expire"
          }
          description  = "remove old images"
          rulePriority = 1
          selection = {
            countNumber = 3
            countType   = "imageCountMoreThan"
            tagStatus   = "any"
          }
        },
      ]
    }
  )
  repository = "leafsteroids/${local.website_name}"
}

resource "aws_ecs_cluster" "leafsteroids_website" {
  name = "leafsteroids"
  tags = local.tags

  setting {
    name  = "containerInsights"
    value = "enabled"
  }
}

resource "aws_secretsmanager_secret" "leafsteroids_website" {
  name = "leafsteroids/website-prod"
  tags = local.tags
}

resource "aws_secretsmanager_secret_version" "leafsteroids_website" {
  secret_id     = aws_secretsmanager_secret.leafsteroids_website.id
  secret_string = var.mongodb_uri
}

resource "aws_iam_role" "leafsteroids_website_codebuild" {
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "codebuild.amazonaws.com"
          }
          Sid = ""
        },
      ]
      Version = "2008-10-17"
    }
  )
  description           = null
  force_detach_policies = false
  max_session_duration  = 3600
  name                  = "codebuild-${local.website_name}"
  path                  = "/"

  tags = local.tags
}

resource "aws_iam_role" "leafsteroids_website_codepipeline" {
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "codepipeline.amazonaws.com"
          }
          Sid = ""
        },
      ]
      Version = "2012-10-17"
    }
  )
  description           = null
  force_detach_policies = false
  max_session_duration  = 3600
  name                  = "codepipeline-${local.website_name}"
  path                  = "/"

  tags = local.tags
}

resource "aws_cloudwatch_log_group" "leafsteroids_website" {
  name = "/aws/codebuild/${local.website_name}"
  tags = local.tags
}

resource "aws_iam_role_policy" "leafsteroids_website_codepipeline" {
  name = "codepipeline-${local.website_name}"
  role = aws_iam_role.leafsteroids_website_codepipeline.name

  policy = jsonencode(
    {
      "Statement" : [
        {
          "Action" : [
            "iam:PassRole"
          ],
          "Resource" : "*",
          "Effect" : "Allow",
          "Condition" : {
            "StringEqualsIfExists" : {
              "iam:PassedToService" : [
                "cloudformation.amazonaws.com",
                "elasticbeanstalk.amazonaws.com",
                "ec2.amazonaws.com",
                "ecs-tasks.amazonaws.com"
              ]
            }
          }
        },
        {
          "Action" : [
            "codecommit:CancelUploadArchive",
            "codecommit:GetBranch",
            "codecommit:GetCommit",
            "codecommit:GetRepository",
            "codecommit:GetUploadArchiveStatus",
            "codecommit:UploadArchive"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "codedeploy:CreateDeployment",
            "codedeploy:GetApplication",
            "codedeploy:GetApplicationRevision",
            "codedeploy:GetDeployment",
            "codedeploy:GetDeploymentConfig",
            "codedeploy:RegisterApplicationRevision"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "codestar-connections:UseConnection"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "elasticbeanstalk:*",
            "ec2:*",
            "elasticloadbalancing:*",
            "autoscaling:*",
            "cloudwatch:*",
            "s3:*",
            "sns:*",
            "cloudformation:*",
            "rds:*",
            "sqs:*",
            "ecs:*"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "lambda:InvokeFunction",
            "lambda:ListFunctions"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "opsworks:CreateDeployment",
            "opsworks:DescribeApps",
            "opsworks:DescribeCommands",
            "opsworks:DescribeDeployments",
            "opsworks:DescribeInstances",
            "opsworks:DescribeStacks",
            "opsworks:UpdateApp",
            "opsworks:UpdateStack"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "cloudformation:CreateStack",
            "cloudformation:DeleteStack",
            "cloudformation:DescribeStacks",
            "cloudformation:UpdateStack",
            "cloudformation:CreateChangeSet",
            "cloudformation:DeleteChangeSet",
            "cloudformation:DescribeChangeSet",
            "cloudformation:ExecuteChangeSet",
            "cloudformation:SetStackPolicy",
            "cloudformation:ValidateTemplate"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Action" : [
            "codebuild:BatchGetBuilds",
            "codebuild:StartBuild",
            "codebuild:BatchGetBuildBatches",
            "codebuild:StartBuildBatch"
          ],
          "Resource" : "*",
          "Effect" : "Allow"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "devicefarm:ListProjects",
            "devicefarm:ListDevicePools",
            "devicefarm:GetRun",
            "devicefarm:GetUpload",
            "devicefarm:CreateUpload",
            "devicefarm:ScheduleRun"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "servicecatalog:ListProvisioningArtifacts",
            "servicecatalog:CreateProvisioningArtifact",
            "servicecatalog:DescribeProvisioningArtifact",
            "servicecatalog:DeleteProvisioningArtifact",
            "servicecatalog:UpdateProduct"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "cloudformation:ValidateTemplate"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "ecr:DescribeImages"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "states:DescribeExecution",
            "states:DescribeStateMachine",
            "states:StartExecution"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "appconfig:StartDeployment",
            "appconfig:StopDeployment",
            "appconfig:GetDeployment"
          ],
          "Resource" : "*"
        },
        {
          "Effect" : "Allow",
          "Action" : [
            "logs:CreateLogGroup",
            "logs:CreateLogStream",
            "logs:PutLogEvents"
          ],
          "Resource" : [
            aws_cloudwatch_log_group.leafsteroids_website.arn,
            "${aws_cloudwatch_log_group.leafsteroids_website.arn}:log-stream:*"
          ]
        }
      ],
      "Version" : "2012-10-17"
    }
  )



}

resource "aws_iam_role_policy" "leafsteroids_website_codebuild" {
  name = "codebuild-${local.website_name}"
  role = aws_iam_role.leafsteroids_website_codebuild.id

  policy = jsonencode(
    {
      "Version" : "2012-10-17",
      "Statement" : [
        {
          "Sid" : "VisualEditor0",
          "Effect" : "Allow",
          "Action" : "ecr:GetAuthorizationToken",
          "Resource" : "*"
        },
        {
          "Sid" : "VisualEditor1",
          "Effect" : "Allow",
          "Action" : [
            "s3:GetBucketAcl",
            "logs:CreateLogGroup",
            "logs:PutLogEvents",
            "ecr:PutImage",
            "s3:PutObject",
            "s3:GetObject",
            "logs:CreateLogStream",
            "ecr:BatchGetImage",
            "ecr:CompleteLayerUpload",
            "ecr:InitiateLayerUpload",
            "s3:GetBucketLocation",
            "ecr:BatchCheckLayerAvailability",
            "s3:GetObjectVersion",
            "ecr:UploadLayerPart"
          ],
          "Resource" : [
            aws_cloudwatch_log_group.leafsteroids_website.arn,
            "${aws_cloudwatch_log_group.leafsteroids_website.arn}:log-stream:*",
            "${aws_s3_bucket.leafsteroids_website.arn}*",
            "${aws_ecr_repository.leafsteroids-website.arn}*"
          ]
        }
      ]
    }
  )
}

resource "aws_iam_role" "leafsteroids_website" {
  assume_role_policy = jsonencode(
    {
      Statement = [
        {
          Action = "sts:AssumeRole"
          Effect = "Allow"
          Principal = {
            Service = "ecs-tasks.amazonaws.com"
          }
          Sid = ""
        },
      ]
      Version = "2008-10-17"
    }
  )
  description           = null
  force_detach_policies = false
  max_session_duration  = 3600
  name                  = "ecsTaskExecutionRole-${local.website_name}"
  path                  = "/"

  tags = local.tags
}

resource "aws_iam_role_policy_attachment" "leafsteroids_website_service_role" {
  policy_arn = "arn:aws:iam::aws:policy/service-role/AmazonECSTaskExecutionRolePolicy"
  role       = aws_iam_role.leafsteroids_website.name
}

resource "aws_iam_role_policy" "leafsteroids_website" {
  name = "ReadLeafSteroidsSecrets"
  role = aws_iam_role.leafsteroids_website.id

  policy = jsonencode(
    {
      Statement = [
        {
          Action   = "secretsmanager:GetSecretValue"
          Effect   = "Allow"
          Resource = aws_secretsmanager_secret.leafsteroids_website.arn
          Sid      = "VisualEditor0"
        },
      ]
      Version = "2012-10-17"
    }
  )
}

resource "aws_ecs_task_definition" "leafsteroids-website" {
  container_definitions = jsonencode(
    [
      {
        environment = [
          {
            name : "DATABASE_NAME",
            value : var.mongodb_database
          },
          {
            name : "REST_SERVICE_IP",
            value : var.rest_service_ip
          },
          {
            name : "REST_SERVICE_PORT",
            value : tostring(var.rest_service_port)
          },
          {
            name : "ATLAS_CHART_EMBED_DASHBOARD_URL",
            value : var.atlas_chart_embed_dashboard_url
          },
          {
            name : "ATLAS_CHART_ID_EVENT",
            value : var.atlas_chart_id_event
          },
          {
            name : "ATLAS_CHART_ID_PLAYER",
            value : var.atlas_chart_id_player
          },
          {
            name : "ATLAS_CHART_ID_HOME",
            value : var.atlas_chart_id_home
          },
          {
            name : "ATLAS_CHART_ID_SIMILAR",
            value : var.atlas_chart_id_similar
          },
          {
            name : "GAME_CLIENT_PORT",
            value : tostring(var.game_client_port)
          }
        ]
        secrets = [
          {
            name : "CONNECTION_STRING",
            valueFrom : aws_secretsmanager_secret_version.leafsteroids_website.arn
          }
        ]
        environmentFiles = []
        essential        = true
        image            = aws_ecr_repository.leafsteroids-website.repository_url
        logConfiguration = {
          logDriver = "awslogs"
          options = {
            awslogs-create-group  = "true"
            awslogs-group         = "/ecs/leafsteroids"
            awslogs-region        = "eu-west-2"
            awslogs-stream-prefix = "ecs"
            max-buffer-size       = "25m"
            mode                  = "non-blocking"
          }
          secretOptions = []
        }
        mountPoints = []
        name        = local.website_name
        portMappings = [
          {
            appProtocol   = "http"
            containerPort = 80
            hostPort      = 80
            name          = "${local.website_name}-80-tcp"
            protocol      = "tcp"
          },
        ]
        systemControls = []
        ulimits        = []
        volumesFrom    = []
      },
    ]
  )
  cpu                = "1024"
  execution_role_arn = aws_iam_role.leafsteroids_website.arn
  family             = local.website_name
  memory             = "2048"
  network_mode       = "awsvpc"
  requires_compatibilities = [
    "FARGATE",
  ]
  tags = local.tags

  runtime_platform {
    cpu_architecture        = "ARM64"
    operating_system_family = "LINUX"
  }
}

resource "aws_security_group" "leafsteroids_website" {
  description = "Leafsteroids security group"
  egress = [
    {
      cidr_blocks = [
        "0.0.0.0/0",
      ]
      description      = null
      from_port        = 0
      ipv6_cidr_blocks = []
      prefix_list_ids  = []
      protocol         = "-1"
      security_groups  = []
      self             = false
      to_port          = 0
    },
  ]
  ingress = [
    {
      cidr_blocks = [
        "0.0.0.0/0",
      ]
      description      = null
      from_port        = 443
      ipv6_cidr_blocks = []
      prefix_list_ids  = []
      protocol         = "tcp"
      security_groups  = []
      self             = false
      to_port          = 443
    },
    {
      cidr_blocks = [
        "0.0.0.0/0",
      ]
      description      = null
      from_port        = 80
      ipv6_cidr_blocks = []
      prefix_list_ids  = []
      protocol         = "tcp"
      security_groups  = []
      self             = false
      to_port          = 80
    }
  ]
  name   = "leafsteroids-website-sg"
  tags   = local.tags
  vpc_id = var.vpc_id
}

resource "aws_ecs_service" "leafsteroids-website" {
  cluster                            = aws_ecs_cluster.leafsteroids_website.arn
  deployment_maximum_percent         = 200
  deployment_minimum_healthy_percent = 100
  desired_count                      = 1
  enable_ecs_managed_tags            = true
  enable_execute_command             = false
  health_check_grace_period_seconds  = 0
  launch_type                        = "FARGATE"
  name                               = local.website_name
  propagate_tags                     = "NONE"
  scheduling_strategy                = "REPLICA"
  tags                               = local.tags
  task_definition                    = aws_ecs_task_definition.leafsteroids-website.arn_without_revision
  triggers                           = {}

  deployment_circuit_breaker {
    enable   = true
    rollback = true
  }

  deployment_controller {
    type = "ECS"
  }

  network_configuration {
    assign_public_ip = true
    security_groups = [
      var.security_group_id,
      aws_security_group.leafsteroids_website.id,
    ]
    subnets = var.subnet_ids
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.leafsteroids_website.arn
    container_name   = local.website_name
    container_port   = 80
  }

  lifecycle {
    ignore_changes = [task_definition]
  }
}

resource "aws_codebuild_project" "leafsteroids_website" {
  badge_enabled        = false
  badge_url            = null
  build_timeout        = 60
  description          = null
  name                 = local.website_name
  project_visibility   = "PRIVATE"
  public_project_alias = null
  queued_timeout       = 480
  resource_access_role = null
  service_role         = aws_iam_role.leafsteroids_website_codebuild.arn
  source_version       = null
  tags                 = local.tags

  artifacts {
    artifact_identifier    = null
    bucket_owner_access    = null
    encryption_disabled    = false
    location               = null
    name                   = local.website_name
    namespace_type         = null
    override_artifact_name = false
    packaging              = "NONE"
    path                   = null
    type                   = "CODEPIPELINE"
  }

  cache {
    location = null
    modes    = []
    type     = "NO_CACHE"
  }

  environment {
    certificate                 = null
    compute_type                = "BUILD_GENERAL1_SMALL"
    image                       = "aws/codebuild/amazonlinux2-aarch64-standard:3.0"
    image_pull_credentials_type = "CODEBUILD"
    privileged_mode             = false
    type                        = "ARM_CONTAINER"

    environment_variable {
      name  = "AWS_DEFAULT_REGION"
      type  = "PLAINTEXT"
      value = "eu-west-2"
    }
    environment_variable {
      name  = "AWS_ACCOUNT_ID"
      type  = "PLAINTEXT"
      value = var.account_id
    }
    environment_variable {
      name  = "WEBSITE_IMAGE_REPO_NAME"
      type  = "PLAINTEXT"
      value = "leafsteroids/${local.website_name}"
    }
  }

  logs_config {
    cloudwatch_logs {
      group_name  = null
      status      = "ENABLED"
      stream_name = null
    }
    s3_logs {
      bucket_owner_access = null
      encryption_disabled = false
      location            = null
      status              = "DISABLED"
    }
  }

  source {
    buildspec           = <<-EOT
        version: 0.2

        phases:
          pre_build:
            commands:
              - echo Logging in to Amazon ECR...
              - aws ecr get-login-password --region $AWS_DEFAULT_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com
              - WEBSITE_REPOSITORY_URI=$AWS_ACCOUNT_ID.dkr.ecr.$AWS_DEFAULT_REGION.amazonaws.com/$WEBSITE_IMAGE_REPO_NAME
              - COMMIT_HASH=$(echo $CODEBUILD_RESOLVED_SOURCE_VERSION | cut -c 1-7)
              - IMAGE_TAG=$${COMMIT_HASH:=latest}
          build:
            commands:
              - echo Build started on `date`
              - echo Building the WEBSITE Docker image...
              - cd website
              - docker build -t $WEBSITE_REPOSITORY_URI:latest .
              - docker tag $WEBSITE_REPOSITORY_URI:latest $WEBSITE_REPOSITORY_URI:$IMAGE_TAG      
              - cd ..
          post_build:
            commands:
              - echo Build completed on `date`
              - echo Pushing the WEBSITE Docker image...
              - docker push $WEBSITE_REPOSITORY_URI:latest
              - docker push $WEBSITE_REPOSITORY_URI:$IMAGE_TAG
              - echo Writing image definitions file...
              - printf '[{"name":"${local.website_name}","imageUri":"%s"}]' $WEBSITE_REPOSITORY_URI:$IMAGE_TAG > imagedefinitions.json
        artifacts:
            files: 
              - imagedefinitions.json
        EOT
    git_clone_depth     = 0
    insecure_ssl        = false
    location            = null
    report_build_status = false
    type                = "CODEPIPELINE"
  }
}

resource "aws_s3_bucket" "leafsteroids_website" {
  bucket = local.website_name

  tags = local.tags
}

resource "aws_codepipeline" "leafsteroids_website" {
  execution_mode = "QUEUED"
  name           = local.website_name
  pipeline_type  = "V2"
  role_arn       = aws_iam_role.leafsteroids_website_codepipeline.arn
  tags           = local.tags

  artifact_store {
    location = aws_s3_bucket.leafsteroids_website.bucket
    type     = "S3"
  }

  stage {
    name = "Source"

    action {
      category = "Source"
      configuration = {
        "BranchName"           = var.branch_name
        "ConnectionArn"        = var.connection_arn
        "DetectChanges"        = "false"
        "FullRepositoryId"     = "mongodb-developer/leafsteroids"
        "OutputArtifactFormat" = "CODE_ZIP"
      }
      input_artifacts = []
      name            = "Source"
      namespace       = "SourceVariables"
      output_artifacts = [
        "SourceArtifact",
      ]
      owner     = "AWS"
      provider  = "CodeStarSourceConnection"
      region    = "eu-west-2"
      role_arn  = null
      run_order = 1
      version   = "1"
    }
  }
  stage {
    name = "Build"

    action {
      category = "Build"
      configuration = {
        "ProjectName" = local.website_name
      }
      input_artifacts = [
        "SourceArtifact",
      ]
      name      = "Build"
      namespace = "BuildVariables"
      output_artifacts = [
        "BuildArtifact",
      ]
      owner     = "AWS"
      provider  = "CodeBuild"
      region    = "eu-west-2"
      role_arn  = null
      run_order = 1
      version   = "1"
    }
  }
  stage {
    name = "Deploy"

    action {
      category = "Deploy"
      configuration = {
        "ClusterName" = aws_ecs_cluster.leafsteroids_website.name
        "ServiceName" = aws_ecs_service.leafsteroids-website.name
      }
      input_artifacts = [
        "BuildArtifact",
      ]
      name             = "Deploy"
      namespace        = "DeployVariables"
      output_artifacts = []
      owner            = "AWS"
      provider         = "ECS"
      region           = "eu-west-2"
      role_arn         = null
      run_order        = 1
      version          = "1"
    }
  }

  trigger {
    provider_type = "CodeStarSourceConnection"

    git_configuration {
      source_action_name = "Source"

      push {
        branches {
          includes = [
            "feature/dockerize",
          ]
        }
      }
    }
  }
}

resource "aws_lb_target_group" "leafsteroids_website" {
  deregistration_delay              = "300"
  ip_address_type                   = "ipv4"
  load_balancing_algorithm_type     = "round_robin"
  load_balancing_anomaly_mitigation = "off"
  load_balancing_cross_zone_enabled = "use_load_balancer_configuration"
  name                              = local.website_name
  name_prefix                       = null
  port                              = 80
  protocol                          = "HTTP"
  protocol_version                  = "HTTP1"
  slow_start                        = 0
  tags                              = local.tags
  target_type                       = "ip"
  vpc_id                            = var.vpc_id

  health_check {
    enabled             = true
    healthy_threshold   = 5
    interval            = 30
    matcher             = "200"
    path                = "/"
    port                = "traffic-port"
    protocol            = "HTTP"
    timeout             = 5
    unhealthy_threshold = 2
  }

  stickiness {
    cookie_duration = 86400
    cookie_name     = null
    enabled         = false
    type            = "lb_cookie"
  }

  target_group_health {
    dns_failover {
      minimum_healthy_targets_count      = "1"
      minimum_healthy_targets_percentage = "off"
    }
    unhealthy_state_routing {
      minimum_healthy_targets_count      = 1
      minimum_healthy_targets_percentage = "off"
    }
  }

}

resource "aws_lb" "leafsteroids_website" {
  client_keep_alive                                            = 3600
  customer_owned_ipv4_pool                                     = null
  desync_mitigation_mode                                       = "defensive"
  drop_invalid_header_fields                                   = false
  enable_cross_zone_load_balancing                             = true
  enable_deletion_protection                                   = false
  enable_http2                                                 = true
  enable_tls_version_and_cipher_suite_headers                  = false
  enable_waf_fail_open                                         = false
  enable_xff_client_port                                       = false
  enable_zonal_shift                                           = false
  enforce_security_group_inbound_rules_on_private_link_traffic = null
  idle_timeout                                                 = 60
  internal                                                     = false
  ip_address_type                                              = "ipv4"
  load_balancer_type                                           = "application"
  name                                                         = local.website_name
  name_prefix                                                  = null
  preserve_host_header                                         = false
  security_groups = [
    var.security_group_id,
    aws_security_group.leafsteroids_website.id,
  ]
  xff_header_processing_mode = "append"
  tags                       = local.tags

  subnets = var.subnet_ids

}

resource "aws_lb_listener" "leafsteroids_website" {
  load_balancer_arn = aws_lb.leafsteroids_website.arn
  port              = 80
  protocol          = "HTTP"
  ssl_policy        = null

  tags = local.tags
  default_action {
    target_group_arn = aws_lb_target_group.leafsteroids_website.arn
    type             = "forward"
  }
}

resource "aws_lb_listener" "leafsteroids_website_https" {
  load_balancer_arn = aws_lb.leafsteroids_website.arn
  port              = 443
  protocol          = "HTTPS"
  ssl_policy        = "ELBSecurityPolicy-2016-08"
  certificate_arn   = aws_acm_certificate.leafsteroids.arn

  tags = local.tags
  default_action {
    target_group_arn = aws_lb_target_group.leafsteroids_website.arn
    type             = "forward"
  }
}

