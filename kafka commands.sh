kafka-topics --create --zookeeper zookeeper:2181 --replication-factor 1 --partitions 1 --topic YouBuyyClichStreamData
kafka-console-producer --broker-list localhost:9092 --topic YouBuyyClichStreamData