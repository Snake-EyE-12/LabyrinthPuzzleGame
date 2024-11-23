using System.Collections;
using System.Collections.Generic;
using Capstone.DataLoad;
using UnityEngine;

namespace Manipulations
{
    public abstract class Method
    {
        public static Method Load(ManipulationData data)
        {
            switch (data.Method)
            {
                case "Add":
                    return new AddMethod();
                case "Remove":
                    return new RemoveMethod();
                case "Set":
                    return new SetMethod();
                default:
                    return new NullMethod();
            }
        }

        public abstract bool MustSet();
        public abstract bool MustAdd();
        public abstract bool MustRemove();
    }
    public class AddMethod : Method
    {
        public override bool MustSet()
        {
            return false;
        }

        public override bool MustAdd()
        {
            return true;
        }

        public override bool MustRemove()
        {
            return false;
        }
    }
    public class RemoveMethod : Method
    {
        public override bool MustSet()
        {
            return false;
        }

        public override bool MustAdd()
        {
            return false;
        }

        public override bool MustRemove()
        {
            return true;
        }
    }
    public class SetMethod : Method
    {
        public override bool MustSet()
        {
            return true;
        }

        public override bool MustAdd()
        {
            return false;
        }

        public override bool MustRemove()
        {
            return false;
        }
    }
    
    public class NullMethod : Method
    {
        public override bool MustSet()
        {
            return false;
        }
        public override bool MustAdd()
        {
            return false;
        }

        public override bool MustRemove()
        {
            return false;
        }
    }
}
