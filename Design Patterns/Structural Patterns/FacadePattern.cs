using System;
using System.Threading;

namespace Design_Patterns.Structural_Patterns
{
    public class FacadePattern
    {
        /*
         * A facade is a class that provides a simple interface
         * to a complex subsystem which contains lots of moving parts.
         * A facade might provide limited functionality in comparison
         * to working with the subsystem directly. However, it includes
         * only those features that clients really care about.
         *
         * REAL WORLD ANALOGY:
         * - When you call a shop to place a phone order, an operator
         * is your facade tp all services and departments of the shop
         * The operator provides you with a simple voice interface
         * to the ordering system, payment gateways, and various delivery services
         */
        public static void Test()
        {
            FacilitiesFacade facilities = new FacilitiesFacade();
            
            facilities.SubmitNetworkRequest();
            
            //Keep checking until job is complete

            while (!facilities.CheckOnStatus()) ;
            
            Console.WriteLine($"Job completed after only {facilities.GetNumberOfCalls()} phone calls");
        }
    }

    class MisDepartment
    {
        private enum States
        {
           Received, DenyAllKnowledge, ReferClientToFacilities,
           FacilitiesHaveNotSentPaperwork, ElectricianIsNotDone,
           ElectricianDidItWrong, DispatchTechnician, SignedOff, DoesNotWork,
           FixElectricianWiring, Complete
        }
        private States m_State;
        
        public void SubmitNetworkRequest()
        {
            m_State = 0;
        }

        public bool CheckOnStatus()
        {
            m_State++;
            if (m_State == States.Complete)
            {
                return true;
            }

            return false;
        }
    }

    class ElectricianUnion
    {
        private enum States
        {
            Received, RejectTheForm, SizeTheJob, SmokeAndJokeBreak,
            WaitForAuthorization, DoTheWrongJob, BlameTheEngineer, WaitToPunchOut,
            DoHalfAJob, ComplainToEngineer, GetClarification, CompleteTheJob,
            TurnInThePaperwork, Complete
        }

        private States m_States;
        
        public void SubmitNetworkRequest()
        {
            m_States = 0;
        }

        public bool CheckOnStatus()
        {
            m_States++;
            if (m_States == States.Complete)
            {
                return true;
            }

            return false;
        }
    }

    class FacilitiesDepartment
    {
        private enum States
        {
            Received, AssignToEngineer, EngineerResearches, RequestIsNoPossible,
            EngineerLeavesCompany, AssignToNewEngineer, NewEngineerResearches,
            EngineerFillOutPaperWork, Complete,
        }

        private States m_States;

        public void SubmitNetworkRequest()
        {
            m_States = 0;
        }

        public bool CheckOnStatus()
        {
            m_States++;
            if (m_States == States.Complete)
            {
                return true;
            }

            return false;
        }
    }

    class FacilitiesFacade
    {
        private enum States
        {
            Received, SubmitToEngineer, SubmitToElectrician, SubmitToTechnician
        }

        private States m_States;
        private int m_Count;

        private FacilitiesDepartment _Engineer = new FacilitiesDepartment();
        private ElectricianUnion _Electrician = new ElectricianUnion();
        private MisDepartment _Technician = new MisDepartment();

        public FacilitiesFacade()
        {
            
        }

        public void SubmitNetworkRequest()
        {
            m_States = 0;
        }

        public bool CheckOnStatus()
        {
            m_Count++;
            // Job request has just been received

            if (m_States == States.Received)
            {
                m_States++;
                // Forward the job request to the engineer
                _Engineer.SubmitNetworkRequest();
                
                Console.WriteLine($"Submitted to Facilities - {m_Count} phone calls so far");
            }
            else if (m_States == States.SubmitToEngineer)
            {
                // if engineer is complete, forward to electrician
                if (_Engineer.CheckOnStatus())
                {
                    m_States++;
                    _Electrician.SubmitNetworkRequest();
                    Console.WriteLine($"Submitted to Electrician - {m_Count} phone calls so far");
                }
            }
            else if (m_States == States.SubmitToElectrician)
            {
                // if electrician is complete, forward to technician
                if (_Electrician.CheckOnStatus())
                {
                    m_States++;
                    _Technician.SubmitNetworkRequest();
                    Console.WriteLine($"submitted to MIS - {m_Count} phone calls so far");
                }
            }
            else if (m_States == States.SubmitToTechnician)
            {
                // if technician is complete, job is done
                if (_Technician.CheckOnStatus())
                {
                    return true;
                }
            }

            // the job is not entirely complete
            return false;
        }

        public int GetNumberOfCalls()
        {
            return m_Count;
        }
    }
}