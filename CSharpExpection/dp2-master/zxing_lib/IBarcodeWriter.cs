﻿/*
 * Copyright 2012 ZXing.Net authors
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

#if !(SILVERLIGHT || NETFX_CORE)
#if !UNITY
using System.Drawing;
#else
using UnityEngine;
#endif
#elif NETFX_CORE
using Windows.UI.Xaml.Media.Imaging;
#else
using System.Windows.Media.Imaging;
#endif

using ZXing.Common;

namespace ZXing
{
   /// <summary>
   /// Interface for a smart class to encode some content into a barcode
   /// </summary>
   public interface IBarcodeWriter
   {
      /// <summary>
      /// Encodes the specified contents.
      /// </summary>
      /// <param name="contents">The contents.</param>
      /// <returns></returns>
      BitMatrix Encode(string contents);

#if MONOTOUCH
      /// <summary>
      /// Creates a visual representation of the contents
      /// </summary>
      MonoTouch.UIKit.UIImage Write(string contents);
#elif MONOANDROID
      /// <summary>
      /// Creates a visual representation of the contents
      /// </summary>
      Android.Graphics.Bitmap Write(string contents);
#else
#if !(SILVERLIGHT || NETFX_CORE)
#if !UNITY
      /// <summary>
      /// Creates a visual representation of the contents
      /// </summary>
      Bitmap Write(string contents);
#else
      /// <summary>
      /// Creates a visual representation of the contents
      /// </summary>
      Color32[] Write(string contents);
#endif
#else
      /// <summary>
      /// Creates a visual representation of the contents
      /// </summary>
      WriteableBitmap Write(string contents);
#endif
#endif
   }
}
