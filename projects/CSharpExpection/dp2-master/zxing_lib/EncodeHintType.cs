/*
* Copyright 2008 ZXing authors
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*      http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

namespace ZXing
{
   /// <summary>
   /// These are a set of hints that you may pass to Writers to specify their behavior.
   /// </summary>
   /// <author>dswitkin@google.com (Daniel Switkin)</author>
   public enum EncodeHintType
   {
      /// <summary>
      /// Specifies the width of the barcode image
      /// type: <see cref="System.Int32" />
      /// </summary>
      WIDTH,

      /// <summary>
      /// Specifies the height of the barcode image
      /// type: <see cref="System.Int32" />
      /// </summary>
      HEIGHT,

      /// <summary>
      /// Don't put the content string into the output image.
      /// type: <see cref="System.Boolean" />
      /// </summary>
      PURE_BARCODE,

      /// <summary>
      /// Specifies what degree of error correction to use, for example in QR Codes.
      /// Type depends on the encoder. For example for QR codes it's type
      /// <see cref="ZXing.QrCode.Internal.ErrorCorrectionLevel" />
      /// </summary>
      ERROR_CORRECTION,

      /// <summary>
      /// Specifies what character encoding to use where applicable.
      /// type: <see cref="System.String" />
      /// </summary>
      CHARACTER_SET,

      /// <summary>
      /// Specifies margin, in pixels, to use when generating the barcode. The meaning can vary
      /// by format; for example it controls margin before and after the barcode horizontally for
      /// most 1D formats.
      /// type: <see cref="System.Int32" />
      /// </summary>
      MARGIN,

      /// <summary>
      /// Specifies whether to use compact mode for PDF417.
      /// type: <see cref="System.Boolean" />
      /// </summary>
      PDF417_COMPACT,

      /// <summary>
      /// Specifies what compaction mode to use for PDF417.
      /// type: <see cref="ZXing.PDF417.Internal.Compaction" />
      /// </summary>
      PDF417_COMPACTION,

      /// <summary>
      /// Specifies the minimum and maximum number of rows and columns for PDF417.
      /// type: <see cref="ZXing.PDF417.Internal.Dimensions" />
      /// </summary>
      PDF417_DIMENSIONS,

      /// <summary>
      /// http://zxingnet.codeplex.com/discussions/399045
      /// Don't append ECI segment.
      /// type: <see cref="System.Boolean" />
      /// </summary>
      DISABLE_ECI,
   }
}