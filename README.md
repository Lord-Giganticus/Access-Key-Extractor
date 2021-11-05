# Access Key Extractor

Extracts server access keys from 3DS and WiiU ROM dumps

Highly inspired by the work done from [PretendoNetwork/access-key-extractor](https://github.com/PretendoNetwork/access-key-extractor/)

Notes are taken from their README.

## Usage

`Extractor.exe [path]`

## Arguments

- `path` The path to the game dump (WiiU ROM decompressed ELF or 3DS ROM bin) [Required]

## Notes

- Encoding defaults to utf16le (`System.Text.Encoding.Unicode`), which seems to work for most titles. Not confrimed working on every title however.

- Access keys are always 8 lowercase characters exept for the Friends app.

- To get the access key for a WiiU title you must first decompress the RPX file in the `code/` folder of the decrypted title into an ELF, then run the extractor on the ELF. See [wiiurpxtool](https://github.com/0CBH0/wiiurpxtool)