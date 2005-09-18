//
// BuildWhen.cs: Represents <When>.
//
// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
// 
// (C) 2005 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections;
using System.Xml;

namespace Microsoft.Build.BuildEngine {
	internal class BuildWhen {
		XmlAttribute		condition;
		Project			parentProject;
		GroupingCollection	groupingCollection;
		XmlElement		whenElement;
	
		public BuildWhen (Project parentProject)
		{
			this.parentProject = parentProject;
			this.groupingCollection = new GroupingCollection ();
		}
		
		public void BindToXml (XmlElement whenElement)
		{
			if (whenElement == null)
				throw new ArgumentNullException ("whenElement");
			this.whenElement = whenElement;
			if (whenElement.GetAttribute ("Condition") != String.Empty)
				condition = whenElement.GetAttributeNode ("Condition");
			foreach (XmlElement xe in whenElement.ChildNodes) {
				if (xe.Name == "ItemGroup") {
					BuildItemGroup big = new BuildItemGroup ();
					//big.BindToXml (xe);
					groupingCollection.Add (big);
				// FIXME: add nested chooses
				} else if (xe.Name == "PropertyGroup") {
					BuildPropertyGroup bpg = new BuildPropertyGroup ();
					//bpg.BindToXml (xe);
					groupingCollection.Add (bpg);
				} else
					throw new InvalidProjectFileException ("Invalid element in When.");
			}
		}
		
		public string Condition {
			get { return condition.Value; }
			set { condition.Value = value; }
		}
		
		public GroupingCollection GroupingCollection {
			get { return groupingCollection; }
			set { groupingCollection = value; }
		}
	}
}
