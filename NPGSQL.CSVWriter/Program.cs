// See https://aka.ms/new-console-template for more information

using Npgsql;
using NPGSQL.CSVWriter;

List<UploadEntry> uploadEntries =
[
    new UploadEntry(
        "csvupload",
        "data1.csv",
        "Books",
        []
    ),
    new UploadEntry(
        "csvupload2",
        "data2.csv",
        "OtherBooks",
        [1]
    )
];

List<UploadEntry2> uploadEntries2 =
[
    new UploadEntry2(
        "csvupload",
        "data1.csv",
        "Books",
        []
    ),
    new UploadEntry2(
        "csvupload2",
        "data2.csv",
        "OtherBooks",
        [1]
    )
];


var implementation = new Implementation();
var implementation2 = new Implementation2();

// await implementation.WriteCsvGeneration(uploadEntries);

await implementation2.WriteCsvGeneration(uploadEntries2);
