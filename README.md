# dapr-workflows-aks-sample
Dapr workflows sample on AKS

## Running this sample

### Prepare a docker image
```bash
docker build -t ghcr.io/shubham1172/dwf-sample:0.1.0 -f Deploy/Dockerfile .
# Make sure you are logged in to ghcr.io.
docker push ghcr.io/shubham1172/dwf-sample:0.1.0
```

### Deploy to AKS
```bash
# Make sure that Dapr is installed on the cluster.
# If not, run dapr init -k
# Install Redis on the cluster.
helm repo add bitnami https://charts.bitnami.com/bitnami
helm install redis bitnami/redis
k apply -f Deploy/redis.yaml

# Install the sample app.
k apply -f Deploy/deployment.yaml

# Expose the Dapr sidecar (only for testing!) and the sample app.
k apply -f Deploy/service.yaml
export SAMPLE_APP_URL=$(k get svc/workflows-sample -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
export DAPR_URL=$(k get svc/workflows-sample-dapr -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
```

### Run the sample
```bash
# Make an API call to the sample app to restock items in the inventory (before starting the workflow).
curl -X GET $SAMPLE_APP_URL/stock/restock

# Start the workflow.
curl -i -X POST $DAPR_URL/v1.0-alpha1/workflows/dapr/OrderProcessingWorkflow/1234/start \
  -H "Content-Type: application/json" \
  -d '{ "input" : {"Name": "Paperclips", "TotalCost": 99.95, "Quantity": 1}}'

# Check the status of the workflow.
curl -i -X GET $DAPR_URL/v1.0-alpha1/workflows/dapr/OrderProcessingWorkflow/1234
```