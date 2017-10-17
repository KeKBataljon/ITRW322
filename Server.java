

import java.io.*; 
import java.net.*; 

class Server 
{ 
static int i=0;  
private static int maxcon=0; 
    
    public static void main(String args[]) 
    { 
    try 
        { 
        ServerSocket ss; 
        Socket s; 

        System.out.println("Server Started"); 
        ss=new ServerSocket(7860); 

            while((i++ < maxcon) || (maxcon == 0)) 
            { 
            doComms connection; 
            s=ss.accept(); 
            System.out.println(s); 
            System.out.println("Client "+i+"  Connected"); 
            doComms conn_c= new doComms(s); 
            Thread t = new Thread(conn_c); 
            t.start(); 
            } 
        } catch (IOException ioe) { 
                                System.out.println("IOException on socket listen: " + ioe); 
                                ioe.printStackTrace(); 
                                  } 
                             
    } 
} 


class doComms implements Runnable  
{ 
    private Socket s; 
    
    doComms(Socket s) 
    { 
      this.s=s; 
    } 

    public void run ()  
    { 

        try { 
        // Get input from the client 
            DataInputStream dis = new DataInputStream (s.getInputStream()); 
            PrintStream out1 = new PrintStream(s.getOutputStream()); 

            String str,extn=""; 
            str=dis.readUTF(); 
            System.out.println("\n"+str); 
            int flag=0,i; 
             
                for(i=0;i<str.length();i++) 
                { 
                     
                    if(str.charAt(i)=='.' || flag==1) 
                    { 
                    flag=1; 
                    extn+=str.charAt(i); 
                    } 
                } 
         
         
//**********************reading image*********************************//             
             
                if(extn.equals(".jpg") || extn.equals(".png")) 
                  {             
                    File file = new File("RecievedImage"+str); 
                    FileOutputStream fout = new FileOutputStream(file); 
              
                    //receive and save image from client 
                    byte[] readData = new byte[1024]; 
                    while((i = dis.read(readData)) != -1) 
                    {
                        fout.write(readData, 0, i); 
                        if(flag==1) 
                        { 
                        System.out.println("Image Has Been Received"); 
                        flag=0; 
                        } 
                    } 
                fout.flush(); 
                fout.close(); 
  
             
             
//****************************Reading Other Files******************//             
                  } 
                else 
                { 
                    FileWriter fstream = new FileWriter("ReceivedFile"+ str); 
                    PrintWriter out=new PrintWriter(fstream); 

                    do 
                    { 
                    str=dis.readUTF(); 
                    System.out.println(" "+str); 
                    out.println(str); 
                    out.flush(); 
                    if(str==null)break; 
    
                    }while(true); 
         
                    System.out.println("One File Received"); 
                    out.close(); 
                } 
            } catch (IOException ioe) { 
                System.out.println(""); 
                                      } 
    } 

}