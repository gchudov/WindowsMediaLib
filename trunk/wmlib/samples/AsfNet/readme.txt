/************************************************************************
AsfNet - Sends the output of DirectShow capture graphs to a network port 
to be read by Windows Media Player

While the underlying libraries are covered by LGPL, this sample is released 
as public domain.  It is distributed in the hope that it will be useful, but 
WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY 
or FITNESS FOR A PARTICULAR PURPOSE.  

**************************************************************************/

In addition to the Windows Media library, you'll also need DirectShowLib (located
at http://directshownet.sourceforge.net

After starting this application, you should be able to connect to the output of 
your capture device by connecting to a TCP/IP port.  From Windows Media Player, go to 
File/Open URL and enter your machine name/ip (for example: http://192.168.0.2:8080).

