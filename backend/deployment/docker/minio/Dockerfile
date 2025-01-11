FROM minio/minio:RELEASE.2024-12-18T13-15-44Z

HEALTHCHECK  --interval=30s --timeout=10s --start-period=10s --start-interval=2s --retries=10 CMD curl -f http://localhost:9000/minio/health/live

CMD ["server","/data","--console-address",":9001"]