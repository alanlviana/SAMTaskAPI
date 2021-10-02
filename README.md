# first-sam-app

To build and deploy this application locally, run the following in your shell in the root of the project:

```bash
docker-compose up -d
aws dynamodb create-table --cli-input-json file://local-environment/create-task-table.json --endpoint-url http://localhost:4566
sam build
sam local start-api --env-vars local-environment/env.json
```

sam deploy --stack-name TasksApi --resolve-s3 --capabilities CAPABILITY_IAM
