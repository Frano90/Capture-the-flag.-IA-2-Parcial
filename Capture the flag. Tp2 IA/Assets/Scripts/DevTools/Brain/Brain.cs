using System;
using DevTools.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Brain
{
   private Queue<Command_Base> currentCommandQueue;
   private Command_Base currentCommand;
   public Enums.BRAIN_STATES currentBrainState = Enums.BRAIN_STATES.Normal;

   
   private List<CommandSequence> _secuencias;
   
   private bool canHaveNewInput = true;
   private float _stunBrain_count;

   private Action OnRecoverBrainBlock;

   private float stunnedTime = 2;
   
   private Dictionary<Enums.INPUT_BRAIN, Command_Base> brain_inputs = new Dictionary<Enums.INPUT_BRAIN, Command_Base>();
   
   public Brain(List<CommandSequence> secuencias)
   {
      OnRecoverBrainBlock += ResumeThink;
      
      _secuencias = secuencias;
      currentCommandQueue = new Queue<Command_Base>();

//      Dejo esto para ver como se hace de modelo      

//      var attack = new RivalCommand_Attack(this, DoNextCommandInQueue);
//      var idle = new RivalCommand_Idle(this, DoNextCommandInQueue, .8f);
//      var moveL = new RivalCommand_Move(this, DoNextCommandInQueue, Enums.BattlePosition.left);
//      var moveR = new RivalCommand_Move(this, DoNextCommandInQueue, Enums.BattlePosition.right);
//      var moveC = new RivalCommand_Move(this, DoNextCommandInQueue, Enums.BattlePosition.center);
      
//      brain_inputs.Add(Enums.INPUT_BRAIN.Move_c, moveC);
//      brain_inputs.Add(Enums.INPUT_BRAIN.Move_l, moveL);
//      brain_inputs.Add(Enums.INPUT_BRAIN.Move_r, moveR);
//      brain_inputs.Add(Enums.INPUT_BRAIN.Attack, attack);
//      brain_inputs.Add(Enums.INPUT_BRAIN.Idle, idle);

   }
   public void DoNextCommandInQueue()
   {
      if (currentCommandQueue.Any())
      {
         currentCommand = currentCommandQueue.Dequeue();
         currentCommand.Init(this,DoNextCommandInQueue);
      }
      else
      {
         var a = Random.Range(0, _secuencias.Count);
         LoadCommandSequence(_secuencias[a].secuencias);
         DoNextCommandInQueue();
      }
   }

   public void InterruptThink(){ResetBrain();}

   void ResetBrain()
   {
      currentBrainState = Enums.BRAIN_STATES.Stunned;
      currentCommand = null;
      currentCommandQueue.Clear();
   }

   public void ResumeThink()
   {
      currentBrainState = Enums.BRAIN_STATES.Normal;
      DoNextCommandInQueue();
   }
   


   public void Think()
   {
      if (currentBrainState == Enums.BRAIN_STATES.Stunned)
      {
         _stunBrain_count += Time.deltaTime;
         if (_stunBrain_count >= stunnedTime)
         {
            _stunBrain_count = 0;
            OnRecoverBrainBlock?.Invoke();
            return;
         } 
      }
      
      currentCommand?.Execute();
   }
   void LoadCommandSequence(List<Enums.INPUT_BRAIN> secuencias)
   {
      foreach (var command in secuencias)
      {
         currentCommandQueue.Enqueue(brain_inputs[command]);
      }
   }
}



