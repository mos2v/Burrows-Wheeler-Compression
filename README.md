# Burrows-Wheeler Compression
#### Video Demo: https://youtu.be/2JDXyJqFoc4
#### Description:

This project implements a text compression and decompression tool using a combination of the Burrows-Wheeler Transform (BWT), Move-to-Front (MTF) encoding, and Huffman coding. The goal is to demonstrate how these algorithms can be combined to achieve efficient data compression, particularly for large text files. This project showcases the complete pipeline from the transformation and encoding of text data to its compression, and finally, its reconstruction from the compressed form.

### Project Structure

- **Program.cs**: The main C# file containing the implementation of the Burrows-Wheeler Transform, Move-to-Front encoding, Huffman encoding, and their respective decoding algorithms. The file also includes helper methods for reading from and writing to files, as well as the main program that ties all components together for compressing and decompressing text files.

### Key Components

1. **Burrows-Wheeler Transform (BWT)**:
   - The BWT is a reversible transformation that rearranges the characters of the input text into runs of similar characters, which makes it more amenable to compression.
   - The method `Burrows` generates the BWT of the input text and returns both the transformed string and the index of the original string within the sorted rotations.
   - The inverse transformation is performed by `Burrows_Inverse`, which reconstructs the original text from the BWT output and the index.

2. **Move-to-Front (MTF) Encoding**:
   - MTF encoding is used to transform the BWT output into a sequence of integers that can be more efficiently compressed. It works by maintaining a list of symbols and moving the most recently accessed symbol to the front of the list.
   - The method `moveToFront` encodes the input string using the MTF algorithm, while `moveToFront_inverse` decodes the MTF output back into a string.

3. **Huffman Encoding**:
   - Huffman encoding is a lossless data compression algorithm that assigns variable-length codes to input characters, with shorter codes assigned to more frequent characters.
   - The method `Huffman_encoding` builds a Huffman tree from the MTF-encoded data, and `GenerateHuffmanCodes` generates the corresponding Huffman codes.
   - `EncodeData` encodes the MTF-encoded data using the Huffman codes, while `DecodeData` decodes the Huffman-encoded data back into the original sequence of integers.

4. **File Operations**:
   - `SaveToBinaryFile` and `ReadFromBinaryFile` handle saving and reading the encoded data to and from binary files, which allows the compressed data to be stored and transmitted efficiently.

### How It Works

1. The program reads a text file using Latin-1 encoding.
2. It applies the Burrows-Wheeler Transform to the text to generate a transformed version that is easier to compress.
3. The transformed text is then encoded using the Move-to-Front algorithm to generate a sequence of integers.
4. Huffman encoding is applied to compress the sequence of integers into a compact binary representation.
5. The compressed data is saved to a binary file.
6. For decompression, the program reads the binary file, decodes the Huffman codes, applies the Move-to-Front decoding, and finally, performs the inverse Burrows-Wheeler Transform to reconstruct the original text.
7. The reconstructed text is saved to a new file, ensuring it matches the original input.

### Design Choices

- **Use of Latin-1 Encoding**: The project uses Latin-1 encoding to handle extended ASCII characters, allowing it to work with a wider range of text inputs beyond the basic ASCII set.
- **Separate Encoding and Compression Steps**: Each step in the compression pipeline (BWT, MTF, Huffman) is implemented as a distinct function, providing modularity and ease of debugging.
- **Handling of Padding in Huffman Encoding**: To ensure the binary data aligns with byte boundaries, padding bits are added during encoding and appropriately removed during decoding.

### Challenges and Considerations

- **Efficiency**: While the implemented algorithms are theoretically efficient, practical performance can be impacted by factors such as input size and memory usage, especially when dealing with large text files.
- **Error Handling**: The current implementation assumes well-formed input and does not explicitly handle all potential error cases, such as corrupted input files or unsupported characters.
- **Future Improvements**: Possible enhancements include optimizing the Huffman tree construction, improving error handling, and adding support for additional encoding schemes or compression methods.

This project serves as a comprehensive example of using classical data compression techniques to efficiently compress and decompress text data, illustrating both the theoretical and practical aspects of these algorithms.
