#include <stdio.h>
#include <stdlib.h>
#include <readline/readline.h>
#include <readline/history.h>
int main(int argc,char **argv){
    puts("Lispy Version 0.1");
    puts("Press Ctrl+c to Exit\n");
    while (1)
    {
        char* input = readline("lisp> ");
        add_history(input);
        printf("Now you are a %s\n",input);
        free(input);
    }
    return 0;
}