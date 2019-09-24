These database scripts expect that you have two databases already created
1. hdastaging - manually created and loaded with data from GT.M
2. hdacore - created and maintained by EF Models in this project

hdastaging_hdacore
Transforms data from hdastaging to hdacore

hdacore_totals
Aggregates data into totals tables for visualizations