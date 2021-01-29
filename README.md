# DataConveyer_FilterLargeCsvData

DataConveyer_FilterLargeCsvData is a sample console application to filter a very large CSV file, so that only records of
interest are retained.
It uses Data Conveyer to perform the conversion.

The application assumes the input file to be similar to the
["Medicare Part D Opioid Prescriber Summary File 2016.csv"](https://data.cms.gov/Medicare-Part-D/Medicare-Part-D-Opioid-Prescriber-Summary-File-201/6wg9-kwip)
file that can be downloaded from the [Centers for Medicare & Medicaid Services (CMS) website](https://data.cms.gov/about).
The file is 75MB in size and contains 1.1 million rows. Here are the beginning records of the file:

```
NPI,NPPES Provider Last Name,NPPES Provider First Name,NPPES Provider ZIP Code,NPPES Provider State,Specialty Description,Total Claim Count,Opioid Claim Count,Opioid Prescribing Rate,Long-Acting Opioid Claim Count,Long-Acting Opioid Prescribing Rate
1003000126,ENKESHAFI,ARDALAN,21502,MD,Internal Medicine,545,23,4.22,,
1003000142,KHALIL,RASHID,43623,OH,Anesthesiology,1733,941,54.30,165,17.53
```

Upon execution, a new file will be created that only contains the records for Dentists residing in New Jersey.
This file is expected to contain about 4,500 records.

Note that the output file (unlike the input file) is suitable for an Excel spreadsheet.

## Installation

* Fork this repository and clone it onto your local machine, or

* Download this repository onto your local machine.

## Usage

1. Open DataConveyer_FilterLargeCsvData solution in Visual Studio.

2. Build and run the application, e.g. hit F5.

    - A console window with directions will show up.

3. Copy an input file (Medicare_Part_D_Opioid_Prescriber_Summary_File_2016.csv) into the ...Data\In folder.

    - A message that the file was detected will appear in the console window.

4. Wait for the copy process to complete, then hit any key.

    - The file will get processed as reported in the console window.

5. Review the contents of the output file placed in the ...Data\Out folder.

6. (optional) Repeat steps 3-5 for other additional input file(s).

7. To exit application, hit Enter key into the console window.

**Note:** You may experience *"InitializationError"* upon starting the process too soon in step 4 above.
This is because the file is locked while copying, which causes Data&nbsp;Conveyer to receive an
`IOException` *`(The process cannot access the file ... because it is being used by another process.)`*
when attempting to open it.
If this happens, delete the file and repeat the process.

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[Apache License 2.0](https://choosealicense.com/licenses/apache-2.0/)

## Copyright

```
Copyright © 2019 Mavidian Technologies Limited Liability Company. All Rights Reserved.
```
