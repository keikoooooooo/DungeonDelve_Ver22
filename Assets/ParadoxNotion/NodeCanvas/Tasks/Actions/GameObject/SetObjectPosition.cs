using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace NodeCanvas.Tasks.Actions
{
    [Name("Set Position")]
    [Category("GameObject")]
    [Description("Đặt vị trí đối tượng.")]
    public class SetObjectPosition : ActionTask
    {
        [RequiredField, Tooltip("Đối tượng cần thao tác")]
        public BBParameter<GameObject> setObject;

        public BBParameter<Vector3> toPosition;

        public BBParameter<Vector3> offset;
        
        protected override string info => "Set Position";
        
        protected override void OnExecute()
        {
            if(setObject.value == null) 
                EndAction(false);
            
            setObject.value.transform.position = toPosition.value + offset.value;
            EndAction(true);
        }
    }
    
}

