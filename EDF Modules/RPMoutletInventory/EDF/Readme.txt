EDF require input .csv file with 6 columns:

1.	Brand in SCE
2.	Brand in Turn14
3.	Brand in Premier
4.	Mayer Short
5.	Mayer Long
6.	Brand in Ebay

EDF download SCE export and match each brand from input .csv
On the interface, you must choose which supplier to work with - "premier / turn14 / meyer"
When the appropriate "Create *** inventory" flag is set, EDF creates an inventory file and uploads it to the FTP

turn 14 stores products in 2 warehouses,  so in the created feed part numbers is written twice(1 warehouse = 1 line) 
in the premier similarly, only for 8 warehouse

When the appropriate "Update Price in SCE" flag is set. EDF updates prices from turn14 in SCE and on Ebay(if flagged)
 
-------------------------------------------------------------------------------------
'Update Price in SCE"  from turn14 rules:

Cost -> cost
retail -> msrp
jobber -> jobber
map -> web

and this rule is used for empty prices - http://take.ms/4uz1Z