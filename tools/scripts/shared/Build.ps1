#==================================================================================================
# Copyright 2017 Dirk Lemstra, Fred Weinhaus
# <https://github.com/dlemstra/FredsImageMagickScripts.NET>
#
# Licensed under the ImageMagick License (the "License"); you may not use this file except in 
# compliance with the License. You may obtain a copy of the License at
#
#   http://www.imagemagick.org/script/license.php
#
# Unless required by applicable law or agreed to in writing, software distributed under the
# License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
# express or implied. See the License for the specific language governing permissions and
# limitations under the License.
#==================================================================================================
function Build()
{
  $solution = "FredsImageMagickScripts.NET.sln"
  .\Tools\Programs\nuget.exe restore $solution
  msbuild /m:4 $solution /t:Rebuild ("/p:Configuration=Test,Platform=""Any CPU""")
  CheckExitCode "Failed to build: $solution"
}