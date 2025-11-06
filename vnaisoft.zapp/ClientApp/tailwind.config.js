const path = require('path');
const process = require('process');
const colors = require('tailwindcss/colors');
const defaultTheme = require('tailwindcss/defaultTheme');
const generatePalette = require(path.resolve(__dirname, ('src/@fuse/tailwind/utils/generate-palette')));

/**
 * Custom palettes
 *
 * Uses the generatePalette helper method to generate
 * Tailwind-like color palettes automatically
 */
const customPalettes = {
    brand: generatePalette('#2196F3')
};

/**
 * Themes
 */
const themes = {
    // Default theme is required for theming system to work correctly
    'default': {
        primary  : {
            ...colors.indigo,
            DEFAULT: colors.indigo[600]
        },
        accent   : {
            ...colors.blueGray,
            DEFAULT: colors.blueGray[800]
        },
        warn     : {
            ...colors.red,
            DEFAULT: colors.red[600]
        },
        'on-warn': {
            500: colors.red['50']
        }
    },
    // Rest of the themes will use the 'default' as the base theme
    // and extend them with their given configuration
    'brand' : {
        primary: customPalettes.brand
    },
    'indigo': {
        primary: {
            ...colors.teal,
            DEFAULT: colors.teal[600]
        }
    },
    'rose'  : {
        primary: colors.rose
    },
    'purple': {
        primary: {
            ...colors.purple,
            DEFAULT: colors.purple[600]
        }
    },
    'amber' : {
        primary: colors.amber
    }
};

/**
 * Tailwind configuration
 *
 * @param isProd
 * This will be automatically supplied by the custom Angular builder
 * based on the current environment of the application (prod, dev etc.)
 */
const config = {
    experimental: {},
    future      : {},
    darkMode    : 'class',
    important   : true,
    purge       : {
        enabled: process.env.TAILWIND_MODE === 'build',
        content: ['./src/**/*.{html,ts}'],
        options: {
            safelist: {
                deep: [/^theme/, /^dark/, /^mat/]
            }
        }
    },
    theme       : {
        colors  : {
            transparent: 'transparent',
            current    : 'currentColor',
            black      : colors.black,
            white      : colors.white,
            pink       : colors.pink,
            gray       : colors.blueGray,
            red        : colors.red,
            cyan       : colors.cyan,
            lime       : colors.lime,
            orange     : colors.orange,
            amber      : colors.amber,
            yellow     : colors.yellow,
            green      : colors.green,
            teal       : colors.teal,
            blue       : colors.blue,
            indigo     : colors.indigo,
            purple     : colors.purple
        },
        fontSize: {
            'xs'  : '0.625rem',
            'sm'  : '0.75rem',
            'md'  : '0.8125rem',
            'base': '0.875rem',
            'lg'  : '1rem',
            'xl'  : '1.125rem',
            '2xl' : '1.25rem',
            '3xl' : '1.5rem',
            '4xl' : '2rem',
            '5xl' : '2.25rem',
            '6xl' : '2.5rem',
            '7xl' : '3rem',
            '8xl' : '4rem',
            '9xl' : '6rem',
            '10xl': '8rem'
        },
        screens : {
            print: {'raw': 'print'},
            mobile   : {'max': '639px'},
            sm   : '600px',
            md   : '960px',
            lg   : '1280px',
            xl   : '1440px'
        },
        extend  : {

            boxShadow: {
                'inset-rose-700': 'inset 0 0 5px 0 rgba(159, 28, 59, 0.7)', // Adjust values as needed
                // Or if you want it to be a preset like the default Tailwind shadows:
                'inset-rose-700-sm': 'inset 0 1px 2px 0 rgba(159, 28, 59, 0.5)',
                'inset-rose-700-md': 'inset 0 4px 6px -1px rgba(159, 28, 59, 0.4), inset 0 2px 4px -1px rgba(159, 28, 59, 0.3)',
                'inset-rose-700-lg': 'inset 0 10px 15px -3px rgba(159, 28, 59, 0.3), inset 0 4px 6px -2px rgba(159, 28, 59, 0.2)'
            },
            colors: {
              rose: {
                700: '#be123c' // Make sure rose-700 is defined if not already.
              }
            },
            aspectRatio: {
                auto: 'auto',
                square: '1 / 1',
                video: '16 / 9',
                1: '1',
                2: '2',
                3: '3',
                4: '4',
                5: '5',
                6: '6',
                7: '7',
                8: '8',
                9: '9',
                10: '10',
                11: '11',
                12: '12',
                13: '13',
                14: '14',
                15: '15',
                16: '16',
              },
            animation : {
                'spin-slow': 'spin 3s linear infinite'
            },
            flex      : {
                '0': '0 0 auto',
                '1':  '1 1 0%',
                '2': '2 2 0%'
            },
            fontFamily: {
                sans: `"Inter var", ${defaultTheme.fontFamily.sans.join(',')}`,
                mono: `"IBM Plex Mono", ${defaultTheme.fontFamily.mono.join(',')}`
            },
            opacity   : {
                12: '0.12',
                38: '0.38',
                87: '0.87'
            },
            rotate    : {
                '-270': '270deg',
                '15'  : '15deg',
                '30'  : '30deg',
                '60'  : '60deg',
                '270' : '270deg'
            },
            scale     : {
                '-1': '-1'
            },
            zIndex    : {
                '-1'   : -1,
                '49'   : 49,
                '60'   : 60,
                '70'   : 70,
                '80'   : 80,
                '90'   : 90,
                '99'   : 99,
                '999'  : 999,
                '9999' : 9999,
                '99999': 99999
            },
            spacing   : {
                '13': '3.25rem',
                '15': '3.75rem',
                '18': '4.5rem',
                '22': '5.5rem',
                '26': '6.5rem',
                '30': '7.5rem',
                '50': '12.5rem',
                '90': '22.5rem'
            },
            /**
             * Extended spacing values for width and height utilities.
             * This way, we won't be adding these to other utilities
             * that use 'spacing' config to keep the file size
             * smaller by not generating useless utilities such as
             * p-1/4 or m-480.
             */
            extendedSpacing: {
                // Fractional values
                '1/2'  : '50%',
                '1/3'  : '33.333333%',
                '2/3'  : '66.666667%',
                '1/4'  : '25%',
                '2/4'  : '50%',
                '3/4'  : '75%',
                '1/5'  : '20%',
                '2/5'  : '40%',
                '3/5'  : '60%',
                '4/5'  : '80%',
                '1/6'  : '16.666667%',
                '2/6'  : '33.333333%',
                '3/6'  : '50%',
                '4/6'  : '66.666667%',
                '5/6'  : '83.333333%',
                '1/12' : '8.333333%',
                '2/12' : '16.666667%',
                '3/12' : '25%',
                '4/12' : '33.333333%',
                '5/12' : '41.666667%',
                '6/12' : '50%',
                '7/12' : '58.333333%',
                '8/12' : '66.666667%',
                '9/12' : '75%',
                '10/12': '83.333333%',
                '11/12': '91.666667%',

                // Bigger values
                '100': '25rem',
                '120': '30rem',
                '128': '32rem',
                '140': '35rem',
                '160': '40rem',
                '180': '45rem',
                '192': '48rem',
                '200': '50rem',
                '240': '60rem',
                '256': '64rem',
                '280': '70rem',
                '320': '80rem',
                '360': '90rem',
                '400': '100rem',
                '480': '120rem'
            },
            height         : theme => ({
                ...theme('extendedSpacing')
            }),
            minHeight      : theme => ({
                ...theme('spacing'),
                ...theme('extendedSpacing')
            }),
            maxHeight      : theme => ({
                ...theme('extendedSpacing'),
                none: 'none'
            }),
            width          : theme => ({
                ...theme('extendedSpacing'),
                px100: "100px",
                px200: "200px",
                px160: "160px",
            }),
            minWidth       : theme => ({
                ...theme('spacing'),
                ...theme('extendedSpacing'),
                'md': '960px',
                screen: '100vw'
            }),
            maxWidth       : theme => ({
                ...theme('spacing'),
                ...theme('extendedSpacing'),
                screen: '100vw'
            }),

            // @tailwindcss/typography
            typography: (theme) => ({
                DEFAULT: {
                    css: {
                        color              : 'var(--fuse-text-default)',
                        '[class~="lead"]'  : {
                            color: 'var(--fuse-text-secondary)'
                        },
                        a                  : {
                            color: 'var(--fuse-primary-500)'
                        },
                        strong             : {
                            color: 'var(--fuse-text-default)'
                        },
                        'ol > li::before'  : {
                            color: 'var(--fuse-text-secondary)'
                        },
                        'ul > li::before'  : {
                            backgroundColor: 'var(--fuse-text-hint)'
                        },
                        hr                 : {
                            borderColor: 'var(--fuse-border)'
                        },
                        blockquote         : {
                            color          : 'var(--fuse-text-default)',
                            borderLeftColor: 'var(--fuse-border)'
                        },
                        h1                 : {
                            color: 'var(--fuse-text-default)'
                        },
                        h2                 : {
                            color: 'var(--fuse-text-default)'
                        },
                        h3                 : {
                            color: 'var(--fuse-text-default)'
                        },
                        h4                 : {
                            color: 'var(--fuse-text-default)'
                        },
                        'figure figcaption': {
                            color: 'var(--fuse-text-secondary)'
                        },
                        code               : {
                            color     : 'var(--fuse-text-default)',
                            fontWeight: '500'
                        },
                        'a code'           : {
                            color: 'var(--fuse-primary)'
                        },
                        pre                : {
                            color          : theme('colors.white'),
                            backgroundColor: theme('colors.gray.800')
                        },
                        thead              : {
                            color            : 'var(--fuse-text-default)',
                            borderBottomColor: 'var(--fuse-border)'
                        },
                        'tbody tr'         : {
                            borderBottomColor: 'var(--fuse-border)'
                        }
                    }
                },
                sm     : {
                    css: {
                        code : {
                            fontSize: '1em'
                        },
                        pre  : {
                            fontSize: '1em'
                        },
                        table: {
                            fontSize: '1em'
                        }
                    }
                }
            })
        }
    },
    variants    : {
        accessibility           : [],
        alignContent            : ['responsive'],
        alignItems              : ['responsive'],
        alignSelf               : ['responsive'],
        animation               : [],
        backgroundAttachment    : [],
        backgroundClip          : [],
        backgroundColor         : ['dark', 'responsive', 'group-hover', 'hover', 'focus', 'focus-within'],
        backgroundImage         : [],
        backgroundOpacity       : ['dark', 'hover'],
        backgroundPosition      : [],
        backgroundRepeat        : [],
        backgroundSize          : [],
        borderCollapse          : [],
        borderColor             : ['dark', 'group-hover', 'hover', 'focus', 'focus-within'],
        borderOpacity           : ['group-hover', 'hover'],
        borderRadius            : ['responsive'],
        borderStyle             : [],
        borderWidth             : ['dark', 'responsive', 'first', 'last', 'odd', 'even'],
        boxShadow               : ['dark', 'responsive', 'hover', 'focus-within'],
        boxSizing               : [],
        cursor                  : [],
        display                 : ['dark', 'responsive', 'hover', 'group-hover'],
        divideColor             : ['dark'],
        divideOpacity           : [],
        divideStyle             : [],
        divideWidth             : ['responsive'],
        fill                    : [],
        flex                    : ['responsive'],
        flexDirection           : ['responsive'],
        flexGrow                : ['responsive'],
        flexShrink              : ['responsive'],
        flexWrap                : ['responsive'],
        fontFamily              : [],
        fontSize                : ['responsive'],
        fontSmoothing           : [],
        fontStyle               : ['responsive'],
        fontVariantNumeric      : [],
        fontWeight              : ['responsive'],
        gap                     : ['responsive'],
        gridAutoColumns         : ['responsive'],
        gridAutoFlow            : ['responsive'],
        gridAutoRows            : ['responsive'],
        gridColumn              : ['responsive'],
        gridColumnEnd           : ['responsive'],
        gridColumnStart         : ['responsive'],
        gridRow                 : ['responsive'],
        gridRowEnd              : ['responsive'],
        gridRowStart            : ['responsive'],
        gridTemplateColumns     : ['responsive'],
        gridTemplateRows        : ['responsive'],
        height                  : ['responsive'],
        inset                   : ['responsive'],
        justifyContent          : ['responsive'],
        justifyItems            : ['responsive'],
        justifySelf             : ['responsive'],
        letterSpacing           : ['responsive'],
        lineHeight              : ['responsive'],
        listStylePosition       : [],
        listStyleType           : [],
        margin                  : ['responsive'],
        maxHeight               : ['responsive'],
        maxWidth                : ['responsive'],
        minHeight               : ['responsive'],
        minWidth                : ['responsive'],
        objectFit               : ['responsive'],
        objectPosition          : ['responsive'],
        opacity                 : ['responsive', 'group-hover', 'hover'],
        order                   : ['responsive'],
        outline                 : [],
        overflow                : ['responsive'],
        overscrollBehavior      : ['responsive'],
        padding                 : ['responsive'],
        placeContent            : ['responsive'],
        placeItems              : ['responsive'],
        placeSelf               : ['responsive'],
        pointerEvents           : ['responsive'],
        position                : ['responsive'],
        resize                  : [],
        ringColor               : ['dark', 'group-hover'],
        ringOffsetColor         : ['dark'],
        ringOffsetWidth         : [],
        ringOpacity             : [],
        ringWidth               : [],
        rotate                  : [],
        scale                   : [],
        skew                    : [],
        space                   : ['responsive'],
        stroke                  : ['responsive'],
        strokeWidth             : ['responsive'],
        tableLayout             : ['responsive'],
        textAlign               : ['responsive'],
        textColor               : ['dark', 'group-hover', 'hover'],
        textDecoration          : ['group-hover', 'hover'],
        textOpacity             : ['group-hover', 'hover'],
        textOverflow            : ['responsive'],
        textTransform           : [],
        transform               : [],
        transformOrigin         : [],
        transitionDelay         : [],
        transitionDuration      : [],
        transitionProperty      : [],
        transitionTimingFunction: [],
        translate               : ['hover'],
        userSelect              : ['responsive'],
        visibility              : ['responsive'],
        whitespace              : ['responsive'],
        width                   : ['responsive'],
        wordBreak               : ['responsive'],
        zIndex                  : ['responsive']
    },
    corePlugins : {
        appearance        : false,
        gradientColorStops: true,
        container         : false,
        float             : false,
        clear             : false,
        placeholderColor  : false,
        placeholderOpacity: false,
        aspectRatio       : false,  
        verticalAlign     : false
    },
    plugins     : [

        // Fuse - Tailwind plugins
        require(path.resolve(__dirname, ('src/@fuse/tailwind/plugins/extract-config'))),
        require(path.resolve(__dirname, ('src/@fuse/tailwind/plugins/utilities'))),
        require(path.resolve(__dirname, ('src/@fuse/tailwind/plugins/icon-size'))),
        require(path.resolve(__dirname, ('src/@fuse/tailwind/plugins/theming')))({themes}),

        // Other third party and/or custom plugins
        require('@tailwindcss/typography')({modifiers: ['sm', 'lg']}),
        require('@tailwindcss/aspect-ratio'),
        require('@tailwindcss/line-clamp')
    ]
};

module.exports = config;
