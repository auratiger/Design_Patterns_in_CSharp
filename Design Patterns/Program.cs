﻿using System;
 using Design_Patterns.Behavioral_Patterns.CommandPattern.CompositeCommand;
 using Design_Patterns.Behavioral_Patterns.InterpreterPattern;
 using Design_Patterns.Creational_Patterns;
 using Design_Patterns.Creational_Patterns.BuilderPattern;
 using Design_Patterns.Creational_Patterns.PrototypePattern;
 using Design_Patterns.Structural_Patterns;

 namespace Design_Patterns
{
    class Program
    {
        static void Main(string[] args)
        {
            // BuilderPattern.HtmlBuilderTest();
            
            // PrototypePattern.TestPrototype(); 
            
            // CompositePattern.Test(); 
            
            // DecoratorPattern.Test();
            
            // FacadePattern.Test();
            
            // FlyWeightPattern.Test();
            
            // CompositeCommand.Test();
            
            InterpreterPattern.Test();
        }
    }
}