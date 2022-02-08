# pathfinding
exploring/learning pathfinding algorithms 
### current functionality
- step-by-step simple one-to-one breadth first with parents and costs
#### todo 
- [ ] basic ui 
    - [ ] show the differences between algorithms
    - [x] edit terrain
- [x] settings view
- [x] randomize grid
- [x] basic result monads
- [ ] performance tests
- [x] step by step visualization of solutions
- [x] fully functional breadth first search 
    - [ ] with multiple starts and targets
- [ ] reusable domain for more advanced algorithms
    - understand what makes a good algorithm for one-one, one-many, many-one, many-many
    - what is reusable between them all?
    - when to use them?
#### versioning
~~~
0.06    git, view cleanup
0.05    settings
0.04    pathfinding; simple one-one breadth first 
0.03    ui; display a grid with blocked/walkable tiles
0.02    ui; hello world, routeing for basic input, increment test-counter
0.01    readme; pathfinding domain, resources, todo, checklist
0.00    readme
~~~
##### resources
- [redblobgames](https://www.redblobgames.com/pathfinding/a-star/introduction.html)
- [algorithminsight](https://algorithmsinsight.wordpress.com/graph-theory-2/a-star-in-general/implementing-astar-for-pathfinding/)
- [avalonia funcui](https://avaloniacommunity.github.io/Avalonia.FuncUI.Docs/)
###### scratch
~~~
run once -> solve ger ui-bugg där open inte ändras
run once -> change terrain är borde reseta 
cleanup mellan domain, helpers och types/wrappers
refactora all init
gör mer permanenta helpers för testing / fsi
refactora routes, kommer inte bli så mycket

--- tests efter andra algorithmen
updates
    changesettings
    toggleterrain
    reset
pathfinding
    runonce
    run
views
    background
    arrow
    cost
~~~