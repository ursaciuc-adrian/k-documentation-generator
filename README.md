
  

# K Documentation Generator
This web app takes a *.k file and creates a HTML page based with the generated documentation.

![preview](https://s.nimbusweb.me/attachment/2431062/7u6fr48qxh3pk8irofhr/tYcubzCyXSV8XGak/screenshot-localhost-44368-2019.01.08-17-30-00.png)
# Input

    //title: A simple k script

	//description: Tuxedo cats always looking dapper drool have a lot of grump in yourself because you can't forget to be grumpy and not be like king grumpy cat cats are fats i like to pets them they like to meow back.

	//<introduction>
	///description: Shove bum in owner's face like camera lens plop down in the middle where everybody walks yet cough. You are a captive audience while sitting on the toilet, pet me eat an easter feather as if it were a bird then burp victoriously.
	//</introduction>

	//<syntax>
	///description: Fight an alligator and win intently stare at the same spot soft kitty warm kitty little ball of furr so ptracy.
	module IMP-SYNTAX
	    syntax AExp ::=  Int  // builtin
		        |  Id  // builtin
		        |  String  // builtin
		        |  "++"  Id
		        |  Id  "++"
		        >  left:
		        AExp "*" AExp [left]
		        | AExp "/" AExp [left]
		        >  left:
		        AExp "+" AExp [left, strict]
		        | AExp "-" AExp [left]
		        |  "(" AExp ")" [bracket]
	   endmodule
	 //</syntax>
	 
	//<definition>
	///description: Catty ipsum get my claw stuck in the dog's ear for scratch my tummy actually i hate you now fight me.
	   module IMP
	       imports IMP-SYNTAX  
	       syntax KResult ::=  Bool  |  Int  |  String
	          
	    configuration <k> $PGM:Stmt </k>
	        <env> .Map </env>
	        <store> .Map </store>
	        <stack> .List </stack>
	        <in  stream="stdin"> .List </in>
	        <out  stream="stdout"> .List </out>
	    
	    rule I1:Int  <= I2:Int  => I1 <=Int I2
	    rule I1:Int  + I2:Int  => I1 +Int I2
	      
	    rule S1:Stmt S2:Stmt => S1 ~> S2
	    
	    endmodule
	//</definition>

	//<configuration>
	///description: Catty ipsum get my claw stuck in the dog's ear for scratch my tummy actually i hate you now fight me.
	 configuration 
	    <threads>
	    <thread>
	        <k> $PGM:Stmt </k>
	        <control>
	            <fstack> .List </fstack>
	        </control>
	        <env> .Map </env>
	        <store> .Map </store>
	        <holds> .Map </holds>
	    </thread>
	</threads>
	<env> .Map </env>
	<store> .Map </store>
	<stack> .List </stack>
	<in  stream="stdin"> .List </in>
	<out  stream="stdout"> .List </out>
	//</configuration>

	//<rule>
	///description: Tuxedo cats always looking dapper drool have a lot of grump in yourself because you can't forget to be grumpy and not be like king grumpy cat cats.
	rule <k> (X:Id => V) ...</k>
	        <env>... X |-> L:Int ...</env>
	        <store>... L |-> V:Int ...</store>
	//</rule>

## Output
  ![output](https://s.nimbusweb.me/attachment/2430650/4pllw6fweuwoosnh6mal/mfAqdMBuodeVxW6p/screenshot-localhost-44368-2019.01.08-15-31-08.png)

## Attributes
- Title: `//title: Some title`
- Description: `//description: Some description`

## Sections
A section is defined by opening and closing a tag.

    //<section>
    //</section>

Sections can also contatin some attributes:
- Name: `///name: Name of section` (the default name is the tag name)
- Description: `///description: Some soection description`

All text in the section that is not recognized as an attribute will be considered code.
	
	//<example>
	///description: the following code is an example
		module IMP-SYNTAX
	//</example>

**There can't be two sections with the same tag name!!!**

## Configuration
In order to generate a scheme for a configuration the section tag must contain `configuration`

    //<configuration>
    //</configuration>
    
    //<configuration_2>
    //</configuration_2>
    
Example
   

	  //<configuration>
      ///description: Catty ipsum get my claw stuck in the dog's ear for scratch my tummy actually i hate you now fight me.
      configuration 
          <threads>
          <thread>
              <k> $PGM:Stmt </k>
              <control>
                  <fstack> .List </fstack>
              </control>
              <env> .Map </env>
              <store> .Map </store>
              <holds> .Map </holds>
          </thread>
      </threads>
      <env> .Map </env>
      <store> .Map </store>
      <stack> .List </stack>
      <in  stream="stdin"> .List </in>
      <out  stream="stdout"> .List </out>
      //</configuration>

This will generate the following scheme
![config](https://s.nimbusweb.me/attachment/2430673/ilz8dp2lscechgchxplv/b81BxTPr7wQDetSQ/screenshot-localhost-44368-2019.01.08-15-39-15.png)

## Rules
In order to generate a scheme for a rule the section tag must contain `rule`
   

    //<rule>
    //</rule>
    
    //<rule_2>
    //</rule_2>
Example

    //<rule>
    ///description: Tuxedo cats always looking dapper drool have a lot of grump in yourself because you can't forget to be grumpy and not be like king grumpy cat cats.
    rule <k> (X:Id => V) ...</k>
            <env>... X |-> L:Int ...</env>
            <store>... L |-> V:Int ...</store>
    //</rule>
    
    //<rule_2>
    ///name: Second rule
    ///description: Tuxedo cats always looking dapper drool have a lot of grump in yourself because you can't forget to be grumpy and not be like king grumpy cat cats.
    rule <k> (X:Id => V) ...</k>
            <env>... X |-> L:Int ...</env>
            <store>... L |-> V:Int ...</store>
    //</rule_2>

![rules](https://s.nimbusweb.me/attachment/2430984/qki7hg0h2ib14qtvnimu/OLt3KMQvLxkQ6kmC/screenshot-localhost-44368-2019.01.08-17-06-31.png)