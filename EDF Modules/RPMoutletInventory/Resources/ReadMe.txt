EDF creates inventory feed for all brands from SCE export 

EDF need .csv file in which there are 3 columns - Brand in SCE / Mayer Short / Mayer Long

For each existing brand in SCE, which is also on meyer a request is made for api to meyer
by template "Mayer Short" + "Manufacturer Part Number";

as a result, the file is uploaded to FTP.

------------------------------------------------------------------

With the empty check box "Use Brand File", only the Part Numbers of the SCE is used


version 1.0