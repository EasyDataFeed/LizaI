In customers file required field is - 'Email'

default fileds for update is:


'Website' , 'Phone 1' , 'Billing Company' , 'First name' , 'Last name' , 'Billing Address' , 'City' , 'State' , 'Zip' , 'Country'



for update CRM fields hearer should be started from 'af:' (like 'af:Website: Domain Age') - for account fileds  and 'cf:' (like 'cf:LinkedIn') - for contact fields



if cell in .csv file is empty EDF will skip it

if cell contain value - 'null' EDF will update this custom filed with empty string