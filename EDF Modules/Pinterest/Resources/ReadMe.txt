EDF requires Chrome browser installed.
Move next files - 'chromedriver.exe', 'WebDriver.dll' to /EasyDataFeed folder.

Export file should contain such headers - 'Part Number', 'Brand', 'General Image', 'Spider URL', 'Description', 'Product Title'.

If checkbox 'Use existing export' unchecked then EDF create and download latest export but don't save it on local PC. You can download export(it can be outdated if needed) from backoffice doing the next steps: Settings-> FTP -> feeds -> newSceExport.zip

Checkbox 'Use businesses account' - if you use business account, this flag should be enabled.

Board TItle - takes the information from the product based on the selection to assign proper Board.

Condition file path - to upload products only with 'Name for Board' value. It should contains headers - 'Brand', 'Main Category', 'Sub Category'.
If this file is empty or not used, takes all products from SCE export.
============================================

EDF takes products from SCE store and upload it to pinterest.com
ver 2.8