# sam-task-api

To build and deploy this application locally, run the following in your shell in the root of the project:

```bash
docker-compose up -d
aws dynamodb create-table --cli-input-json file://local-environment/create-task-table.json --endpoint-url http://localhost:4566
sam build
sam local start-api --env-vars local-environment/env.json
```

To build and deploy this application on your own AWS Account, run the following in your shell: ( You will need to have a AWS Cli configured. )

```bash
sam build
sam deploy --stack-name TasksApi --resolve-s3 --capabilities CAPABILITY_IAM
```
