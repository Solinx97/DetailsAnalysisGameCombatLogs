provider "azurerm" {
    features {}
    skip_provider_registration = true
}

terraform {
    required_providers {
        azurerm = {
            source = "hashicorp/azurerm"
            version = "3.75.0"
        }
        # helm = {
        #     source  = "hashicorp/helm"
        #     version = ">= 2.1.0"
        # }
    }
}