# Abstract
Microservice architecture introduces a problem where propagating configuration changes to many services can be difficult to manage. The configuration management system will provide a centralized system for storing and retrieving application configurations as well as pushing those configuration changes out.

## Requirements
- Support users and roles
- Support multiple formats, starting with JSON
- Support configurations for multiple environments
    - Each environment must support access control
- Support webhooks (e.g. call webhook on save)
- Support Git as an option for saving configurations

## Other ideas
- Support configuration inheritance or composition. As a convenience, common configurations can be added to an application's configuration. For example, our NServiceBus configuration is shaped the same way in all applications, so instead of recreating that structure in a different application, simply import the structure. This may even go further than the structure itself, and also include default settings.
    - Should probably limit editing to this structure ad-hoc. For example, an application can add fields to the imported structure but not remove or rename anything.