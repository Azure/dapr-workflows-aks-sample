# dapr-workflows-aks-sample
With Dapr Workflows, you can easily orchestrate messaging, state management, and failure-handling logic across various microservices. Dapr Workflows can help you create long-running, fault-tolerant, and stateful applications.

## Running this sample

### Before you begin

1. Clone the sample project.
1. [Create an AKS cluster.](https://learn.microsoft.com/azure/aks/learn/quick-kubernetes-deploy-cli)
1. [Install the Dapr extension on your AKS cluster.](https://learn.microsoft.com/azure/aks/dapr#prerequisites)

### Deploy to AKS

Make sure that Dapr is installed on the cluster. If not, run dapr init -k.

```bash
helm repo add bitnami https://charts.bitnami.com/bitnami
```

Install Redis on the cluster.

```bash
helm install redis bitnami/redis
k apply -f Deploy/redis.yaml
```

Install the sample app.

```bash
k apply -f Deploy/deployment.yaml
```

Expose the Dapr sidecar (only for testing!) and the sample app.

```bash
k apply -f Deploy/service.yaml
export SAMPLE_APP_URL=$(k get svc/workflows-sample -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
export DAPR_URL=$(k get svc/workflows-sample-dapr -o jsonpath='{.status.loadBalancer.ingress[0].ip}')
```

### Run the sample workflow

Before starting the workflow, make an API call to the sample app to restock items in the inventory.

```bash
curl -X GET $SAMPLE_APP_URL/stock/restock
```

Start the workflow.

```bash
curl -i -X POST $DAPR_URL/v1.0-alpha1/workflows/dapr/OrderProcessingWorkflow/1234/start \
  -H "Content-Type: application/json" \
  -d '{ "input" : {"Name": "Paperclips", "TotalCost": 99.95, "Quantity": 1}}'
```

Check the status of the workflow.

```bash
curl -i -X GET $DAPR_URL/v1.0-alpha1/workflows/dapr/OrderProcessingWorkflow/1234
```

## Related links
- [Dapr Workflow documentation](https://docs.dapr.io/developing-applications/building-blocks/workflow/)
- [Dapr extension for AKS and Arc-enabled Kubernetes documentation](https://learn.microsoft.com/en-us/azure/aks/dapr-overview)