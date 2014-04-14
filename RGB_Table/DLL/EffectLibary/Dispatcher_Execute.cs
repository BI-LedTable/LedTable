using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
namespace RGB_Libary
{

   public delegate void execute_effect();
   
    public class Dispatcher_Execute
    {
       private DispatcherTimer dt;
       execute_effect e_eff;
       public Dispatcher_Execute(DispatcherTimer dt) 
       {
           this.dt = dt;
           init_Timer();
       }

       public execute_effect Execute_Effect
       {
           set 
           { 
               e_eff = value;
               
           }
       }
       private TimeSpan ts = new TimeSpan();

       public TimeSpan Timer_Intervall 
       {
           set  {
                    ts = value;
      
                }
          
       }
       private void init_Timer() 
       {
               //dt.Tick += dt_Tick;
               //if (!dt.IsEnabled )
               //    dt.Start();
       }

       private void dt_Tick(object sender, EventArgs e)
       {
           
           //if(e_eff!=null)
           //e_eff();
       }
    }
}
