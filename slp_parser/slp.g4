grammar slp;

EXPORT
    :   'export'
    ;

CLASS
    :   'class'
    ;

TRUE
    :   'true'
    ;

FAIL
    :   'fail'
    ;

NULL
    :   '<>'
    ;

STRING
    :   '"' ( ~["] )* '"'
    ;

INT
    :   '0'..'9'+
    ;

FLOAT
    :   INT '.' INT
    ;

ID  
    // :   ('a'..'z'|'A'..'Z'|'_') ('a'..'z'|'A'..'Z'|'0'..'9'|'_')*
    :   ('a'..'z'|'A'..'Z'|'0'..'9'|'_')+
    ;

NATIVE
    :   '{' ( NATIVE | ~[{}] )* '}'
    ;

COMMENT
    :   '/*' .*? '*/'    -> channel(HIDDEN) // match anything between /* and */
    ;

LINE_COMMENT
    : '//' ~[\r\n]* '\r'? '\n' -> channel(HIDDEN)
    ;

WS  :   [ \r\t\u000C\n]+ -> channel(HIDDEN)
    ;

program : // predicate+
          // (exp)+
            exp
    ;

predicate
    :   EXPORT? ID '(' inParams=decList? (';' outParams=decList)? ')' ':-' exp '.' 
    ;

decList
    : declaration (',' declaration)*
    ;

declaration
    :   exp
    ;

exp
    :   
       ('<' call '>')+ (id | nat)  #Attribute
    |    (   id
        |   call
        |   nat 
        |   ( INT       
            |   FLOAT
            |   STRING
            |   TRUE
            |   FAIL
            |   NULL
            )
        )                   #Sing
    // |   id '[' exp ']'      #Child
    // |  'export' '(' exp ')' #Export
    |   '(' exp ')'         #Para
    |   exp '.' ID          #Member
    
    //|   ('<' call '>')+ id #Attribute
    |   exp '?'             #Question
    |   '!' exp             #Not
    |   exp '^' exp         #Binary
    |   exp '/' exp         #Binary
    |   exp '*' exp         #Associative
    |   exp '**' exp        #Associative
    |   exp '*?' exp        #Sugar
    |   exp '*!' exp        #Sugar
    |   exp '*-' exp        #Sugar   
    |   '-' exp             #Min
    |   '\\+-' exp          #Min
    |   '+-' exp            #OpVarUnary
    |   exp '-' exp         #Associative
    |   exp '+' exp         #Associative
    |   exp '+-' exp        #OpVar
    |   exp '\\+-' exp      #Associative
    |   exp '-+' exp        #OpVar
    |   exp '++' exp        #Associative
    |   exp '==' exp        #Binary
    |   exp '=' exp         #Binary
    |   exp ':=' exp        #Binary
    |   exp '<' exp         #Binary
    |   exp '>' exp         #Binary
    |   exp '<=' exp        #Binary
    |   exp '>=' exp        #Binary
    |   exp '@' id          #Switch
    |   exp ',' exp         #Associative
    |   exp '::' exp        #Binary
    |   '?->' exp           #Sugar
    |   '-->' exp           #Sugar
    |   '->' exp            #Sugar
    |   exp '?->' exp       #Sugar
    |   exp '-->' exp       #Sugar
    |   exp '->' exp        #Sugar
    |   exp ';' exp         #Associative
    |   exp '||' exp        #Associative
    |   exp ','             #Para
    |   ';' exp             #Para
    |   (   '(' '+' ')'
        |   '(' '-' ')'
        |   '(' '*' ')'
        |   '(' '/' ')'
        )                   #Operator
    //|   exp ':=' exp        #Binary
   //  |   '(' exp exp ')'     #Concatenation
    ;

id
    :   ID
    ;

nat
    :   ID? NATIVE
    ;

call
    :   ('<' call '>')* ID ('(' expList? ')')?      #NormalCall
    |   ('<' call '>')* ID '!' ('(' expList? ')')?  #DeclareCall
    |   ('<' call '>')* ID '[' expList? ']'         #DelayedCall1
    |   ('<' call '>')* ID '?' '(' expList? ')'     #DelayedCall2
    |   ('<' call '>')* ID '<' expList? '>'         #DelayedCall2
    ;

/*
call2
    :   ID '[' expList? ']'
    ;

call3
    :   ID '<' expList? '>'
    ;
*/

expList
    :   exp (',' exp)*
    ;

rul
    :   'export'? exp ':=' exp
    ;