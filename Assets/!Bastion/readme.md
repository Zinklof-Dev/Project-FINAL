BASTION is a drag and drop addition to unity. Seeking to replace vital functions locked behind packages that barely work, are needlessly complicated, or are barely even supported. alongside adding functions, systems, and utilities that should've been in unity from the get go.

Our primary goal is to make tools that have a complicated backend, so you can stay in your front end. While also making these tools as simple as possible to use for you, hopefully so simple a documentation isn't even needed! (though we'll do our best to provide one)


<h2>BASTIONS feature set</h2>

| Feature | Completeness | Support | Consideration |
|:---:|:---:|:---:|:---:|
| Console | Very | High | Primary Feature |
| Settings | Somewhat | High | Primary Feature |
| Localization | High | High | Secondary Feature |
| PCG-XSL-RR 128/64 RNG | In works | High | Secondary Feature |
| Additional Math Library | Somewhat | High | Additional Feature |
| Procedural Mapping Toolset | Somewhat | Medium | Additional Feature |
| Unit Testing | DEPRECATED | Low to None | Backwards Compatability |

<sub>The additional math library contains a performance optimization we use ourselves a lot, so it retains high support. (Distance formula that doesn't do the square root, effectively working in square space, saving a little bit of time).</sub>

<sub>Completeness is a rough estimate of how complete the system is, with high meaning its in working, usuable coniditon, but could use additional feature sets. We will always seek to improve user comfort and user quality of life by adding to features even when they're considered very complete if an addition will improve general workflow.</sub>
<sub>Support is a rough idea of how important a feature working is within the framework. high support features will be the first to be fixed if unity breaks them or we break them, whereas low will be last in the order of fixing</sub>
<sub>Consideration is a general idea of how important to the framework a feature is, and is somewhat releated to its support level. Primary features will be focused on first for improvement, secondary after that, etc. etc.</sub>
